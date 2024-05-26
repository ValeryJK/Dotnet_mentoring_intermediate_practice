namespace EventBookSystem.Common.Models
{
    public class NotificationRequest
    {
        public string? TrackingId { get; set; }
        public string? OperationName { get; set; }
        public DateTime Timestamp { get; set; }
        public string? CustomerEmail => "valerykalasouski@gmail.com";
        public string? CustomerName => "Test";
        public string? Content { get; set; }
    }
}