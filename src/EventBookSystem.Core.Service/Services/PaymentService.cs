using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.Data.Enums;
using EventBookSystem.Data.Repositories.Interfaces;

namespace EventBookSystem.Core.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, ICartRepository cartRepository,
            ILoggerManager logger, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(bool trackChanges)
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync(trackChanges);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);

            if (payment == null)
            {
                _logger.LogWarn($"Payment with ID {paymentId} not found.");

                return default;
            }

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<bool> CompletePaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            var cartsItems = await _cartRepository.GetCartItemsByPaymentId(paymentId);

            if (!cartsItems.Any())
            {
                return false;
            }

            foreach (var item in cartsItems)
            {
                item.Seat.Status = SeatStatus.Sold;
            }

            payment.Status = PaymentStatus.Paid;

            await _paymentRepository.SaveAsync();

            return true;
        }

        public async Task<bool> FailPaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            var cartsItems = await _cartRepository.GetCartItemsByPaymentId(paymentId);

            if (!cartsItems.Any())
            {
                return false;
            }

            foreach (var item in cartsItems)
            {
                item.Seat.Status = SeatStatus.Available;
            }

            payment.Status = PaymentStatus.Failed;

            await _paymentRepository.SaveAsync();

            return true;
        }
    }
}