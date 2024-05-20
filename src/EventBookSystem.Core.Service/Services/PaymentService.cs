using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.Data.Enums;
using EventBookSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentStatus = EventBookSystem.Data.Enums.PaymentStatus;

namespace EventBookSystem.Core.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, ICartRepository cartRepository,
            ILogger<PaymentService> logger, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(bool trackChanges = default)
        {
            var payments = await _paymentRepository.GetAll(trackChanges).OrderBy(c => c.DateUTC).ToListAsync();

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, bool trackChanges = default)
        {
            var payment = await _paymentRepository.GetAll(trackChanges).FirstOrDefaultAsync(x => x.Id == paymentId);

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<bool> CompletePaymentAsync(Guid paymentId, bool trackChanges = true)
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

            _logger.LogInformation("Payment completed successfully.");

            return true;
        }

        public async Task<bool> FailPaymentAsync(Guid paymentId, bool trackChanges = true)
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