using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByBookingIdAsync(int bookingId);
        Task<int> SaveChangesAsync();
    }
}
