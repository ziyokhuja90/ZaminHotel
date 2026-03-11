using ZaminHotel.Domain.Entities;

namespace ZaminHotel.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> PayAsync(int bookingId, int userId);
    }
}
