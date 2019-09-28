namespace SbpExampleShop.Backend.Integration.Models
{
    public class GenerateRequest
    {
        public QrType QrType { get; set; }
        public string MerchantId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string PaymentPurpose { get; set; }
        public string AccountNumber { get; set; }
    }

    public enum QrType
    {
        Static = 1,
        Dynamic = 2
    }
}