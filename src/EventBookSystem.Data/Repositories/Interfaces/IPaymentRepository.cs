using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.Data.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<Payment> GetPaymentByIdAsync(Guid paymentId);
    }
}