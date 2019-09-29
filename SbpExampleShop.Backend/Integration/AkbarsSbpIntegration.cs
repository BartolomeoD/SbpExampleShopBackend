using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SbpExampleShop.Backend.Abstractions;
using SbpExampleShop.Backend.Abstractions.Repositories;
using SbpExampleShop.Backend.Integration.Models;

namespace SbpExampleShop.Backend.Integration
{
    public class AkbarsSbpIntegration : IAkbarsSbpIntegration
    {
        private readonly HttpClient _httpClient;
        private readonly AkbarsSbpIntegrationOptions _options;
        private string _authToken;

        public AkbarsSbpIntegration(HttpClient httpClient, IOptions<AkbarsSbpIntegrationOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<GenerateQrDto> GenerateQr(decimal productPrice, string message)
        {
            var requestData = new GenerateRequest
            {
                Amount = productPrice,
                Currency = "RUB",
                QrType = QrType.Dynamic,
                MerchantId = _options.MerchantId,
                AccountNumber = _options.AccountNumber,
                PaymentPurpose = message
            };

            var request = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8,
                "application/json");
            var token = await GetAuthToken();
            var httpMessage = new HttpRequestMessage
            {
                Content = request,
                Method = HttpMethod.Post,
                RequestUri = new Uri(_options.Url + "/v1/qr/generate"),
                Headers = {{"Authorization", $"Bearer {token}"}}
            };
            var response = await _httpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<GenerateResponse>(responseContent);
            return new GenerateQrDto
            {
                QrId = responseData.QrId,
                Payload = responseData.PayLoad
            };
        }
        
        public async Task<PaymentStatus> GetStatus(string qrId)
        {
            var token = await GetAuthToken();
            var httpMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_options.Url + $"/v1/qr/{qrId}/status"),
                Headers = {{"Authorization", $"Bearer {token}"}}
            };
            var response = await _httpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<StatusResponse>(responseContent);
            switch (responseData.Status)
            {
                case Status.NotStarted:
                case Status.Received:
                case Status.InProgress:
                    return PaymentStatus.Waiting;
                case Status.Accepted:
                    return PaymentStatus.Success;
                case Status.Rejected:
                    return PaymentStatus.Error;
                case Status.NotFound:
                    throw new Exception();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<string> GetAuthToken()
        {
            if (!string.IsNullOrEmpty(_authToken)) 
                return _authToken;
            var requestData = new MultipartFormDataContent
            {
                {new StringContent(_options.Secret), "client_secret"},
                {new StringContent(_options.GrantType), "grant_type"},
                {new StringContent(_options.ClientId), "client_id"},
                {new StringContent(_options.Scope), "scope"}
            };

            var response = await _httpClient.PostAsync(_options.Url + "/identity/connect/token", requestData);
            response.EnsureSuccessStatusCode();
            var responseData =
                JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());
            _authToken = responseData.AccessToken;

            return _authToken;
        }
    }
}