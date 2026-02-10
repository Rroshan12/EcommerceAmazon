using MediatR;
using Discount.Dtos;
using Discount.Entities;
using Discount.Repositories;

namespace Discount.Features.Coupons.Commands
{
    public sealed class UpdateCouponCommand : IRequest<CouponDto>
    {
        public UpdateCouponRequest Request { get; }
        public UpdateCouponCommand(UpdateCouponRequest request) => Request = request;
    }

    public sealed class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, CouponDto>
    {
        private readonly IDiscountRepository _repo;
        public UpdateCouponCommandHandler(IDiscountRepository repo) => _repo = repo;

        public async Task<CouponDto> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = new Coupon
            {
                Id = request.Request.Id,
                ProductId = request.Request.ProductId,
                ProductName = request.Request.ProductName,
                Description = request.Request.Description,
                Amount = request.Request.Amount
            };

            var updated = await _repo.UpdateCouponAsync(coupon);
            if (!updated) return null;

            var updated_coupon = await _repo.GetCouponByIdAsync(coupon.Id);
            return new CouponDto
            {
                Id = updated_coupon.Id,
                ProductId = updated_coupon.ProductId,
                ProductName = updated_coupon.ProductName,
                Description = updated_coupon.Description,
                Amount = updated_coupon.Amount,
                CreatedAt = updated_coupon.CreatedAt,
                UpdatedAt = updated_coupon.UpdatedAt
            };
        }
    }
}
