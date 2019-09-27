namespace SbpExampleShop.Backend.Abstractions
{
    public interface IQrEncoder
    {
        string EncodeToBase64(string generatedQrPayload);
    }
}