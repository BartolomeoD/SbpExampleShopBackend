namespace SbpExampleShop.Backend.Integration.Models
{
    public class StatusResponse
    {
        public string QrId { get; set; }

        public string TransactionId { get; set; }

        public string MerchantId { get; set; }

        public Status Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public decimal Fee { get; set; }
    }

    public enum Status
    {
        NotStarted = 0,
        Received = 1,
        InProgress = 2,
        Accepted = 3,
        Rejected = 4,
        NotFound = 5,
    }
}