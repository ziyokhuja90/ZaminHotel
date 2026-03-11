using ZaminHotel.Application.Interfaces;
using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<Payment> PayAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
            {
                throw new KeyNotFoundException("Booking not found or access denied.");
            }
            if (booking.Status == "Cancelled")
            {
                throw new InvalidOperationException("Cannot pay for a cancelled booking.");
            }

            var existingPayment = await _paymentRepository.GetByBookingIdAsync(bookingId);

            if (existingPayment != null && existingPayment.PaymentStatus == "Paid")
                throw new InvalidOperationException("This booking has already been paid.");

            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                PaymentStatus = "Paid"
            };
            await _paymentRepository.AddAsync(payment);

            booking.Status = "Confirmed";

            _bookingRepository.Update(booking);

            await _paymentRepository.SaveChangesAsync();
            return payment;
        }
    }
}
