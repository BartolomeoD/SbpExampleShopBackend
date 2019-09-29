using System.Threading.Tasks;
using SbpExampleShop.Backend.Abstractions.Repositories;

namespace SbpExampleShop.Backend.Abstractions
{
    public interface IAkbarsSbpIntegration
    {
        Task<GenerateQrDto> GenerateQr(decimal productPrice, string message);
        Task<PaymentStatus> GetStatus(string qrId);
    }
}