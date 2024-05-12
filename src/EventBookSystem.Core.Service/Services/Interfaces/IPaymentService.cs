using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(bool trackChanges);
        Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId);
        Task<bool> CompletePaymentAsync(Guid paymentId);
        Task<bool> FailPaymentAsync(Guid paymentId);
    }
}
