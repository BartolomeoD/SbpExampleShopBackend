namespace SbpExampleShop.Backend.Contracts
{
    public class StatusResponse
    {
        public Status Status { get; set; }
    }

    public enum Status
    {
        Success = 0,
        Error = -1,
        Waiting = 1,
    }
}