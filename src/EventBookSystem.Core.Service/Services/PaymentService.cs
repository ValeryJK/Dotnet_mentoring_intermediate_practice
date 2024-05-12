using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Enums;

namespace EventBookSystem.Core.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PaymentService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(bool trackChanges)
        {
            var payments = await _repository.Payment.GetAllPaymentsAsync(trackChanges);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(paymentId);

            if (payment == null)
            {
                _logger.LogWarn($"Payment with ID {paymentId} not found.");

                return default;
            }

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<bool> CompletePaymentAsync(Guid paymentId)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(paymentId);
            var cartsItems = await _repository.Cart.GetCartItemsByPaymentId(paymentId);

            if (!cartsItems.Any())
            {
                return false;
            }

            foreach (var item in cartsItems)
            {
                item.Seat.Status = SeatStatus.Sold;
            }

            payment.Status = PaymentStatus.Paid;

            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> FailPaymentAsync(Guid paymentId)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(paymentId);
            var cartsItems = await _repository.Cart.GetCartItemsByPaymentId(paymentId);

            if (!cartsItems.Any())
            {
                return false;
            }

            foreach (var item in cartsItems)
            {
                item.Seat.Status = SeatStatus.Available;
            }

            payment.Status = PaymentStatus.Failed;

            await _repository.SaveAsync();

            return true;
        }
    }
}
