using EventBookSystem.Data.Entities;

namespace EventBookSystem.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(bool trackChanges);

        Task<Payment> GetPaymentByIdAsync(Guid paymentId);

        void Update(Payment payment);

        void Create(Payment payment);

        void Delete(Payment payment);
    }
}