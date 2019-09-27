using System.Threading.Tasks;

namespace SbpExampleShop.Backend.Abstractions
{
    public interface IAkbarsSbpIntegration
    {
        Task<GenerateQrDto> GenerateQr(decimal productPrice, string message);
    }
}