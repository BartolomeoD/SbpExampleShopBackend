using System.Threading.Tasks;
using SbpExampleShop.Backend.Abstractions;
using SbpExampleShop.Backend.Abstractions.Repositories;

namespace SbpExampleShop.Backend.Logic
{
    public class PaymentService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAkbarsSbpIntegration _sbpIntegration;
        private readonly IQrEncoder _qrEncoder;
        private readonly PaymentStatusService _statusService;

        public PaymentService(IProductRepository productRepository, IAkbarsSbpIntegration sbpIntegration,
            IQrEncoder qrEncoder, PaymentStatusService statusService)
        {
            _productRepository = productRepository;
            _sbpIntegration = sbpIntegration;
            _qrEncoder = qrEncoder;
            _statusService = statusService;
        }

        public async Task<BuyResultDto> Buy(long productId)
        {
            var product = await _productRepository.Get(productId);
            var message = $"Оплата в онлайн магазине \"Семерочка\", товара \"{product.Name}\"";
            var generatedQr = await _sbpIntegration.GenerateQr(product.Price, message);
            _statusService.AddToChecking(generatedQr.QrId);
            var encodedQr = _qrEncoder.EncodeToBase64(generatedQr.Payload);

            return new BuyResultDto
            {
                EncodedQr = encodedQr,
                QrId = generatedQr.QrId
            };
        }
    }
}