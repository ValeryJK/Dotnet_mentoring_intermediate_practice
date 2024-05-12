using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.Data.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(MainDBContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(bool trackChanges) =>
           await FindAll(trackChanges).OrderBy(c => c.DateUTC).ToListAsync();

        public async Task<Payment> GetPaymentByIdAsync(Guid paymentId)
        {
            return await _context.Payments.FirstAsync(x => x.Id == paymentId);
        }
    }
}
