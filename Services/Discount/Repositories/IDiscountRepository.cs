using Discount.Entities;

namespace Discount.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetCouponByProductIdAsync(string productId);
        Task<Coupon> GetCouponByIdAsync(int id);
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<bool> CreateCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(Coupon coupon);
        Task<bool> DeleteCouponAsync(int id);
    }
}
