
namespace EventBookSystem.Common.DTO
{
    public class UpdatePaymentStatusDto
    {
        public PaymentStatus Status { get; set; }
    }

    public enum PaymentStatus
    {
        Complete = 1,
        Failed = 2,
    }
}