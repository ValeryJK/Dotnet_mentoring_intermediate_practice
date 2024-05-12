namespace EventBookSystem.Common.DTO
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatusDto Status { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime DateUTC { get; set; }
    }

    public class PaymentStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
