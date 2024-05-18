using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(bool trackChanges = false);

        Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, bool trackChanges = false);

        Task<bool> CompletePaymentAsync(Guid paymentId, bool trackChanges = true);

        Task<bool> FailPaymentAsync(Guid paymentId, bool trackChanges = true);
    }
}