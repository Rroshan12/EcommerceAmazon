using MediatR;
using Discount.Dtos;
using Discount.Entities;
using Discount.Repositories;

namespace Discount.Features.Coupons.Commands
{
    public sealed class CreateCouponCommand : IRequest<CouponDto>
    {
        public CreateCouponRequest Request { get; }
        public CreateCouponCommand(CreateCouponRequest request) => Request = request;
    }

    public sealed class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, CouponDto>
    {
        private readonly IDiscountRepository _repo;
        public CreateCouponCommandHandler(IDiscountRepository repo) => _repo = repo;

        public async Task<CouponDto> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var coupon = new Coupon
            {
                ProductId = request.Request.ProductId,
                ProductName = request.Request.ProductName,
                Description = request.Request.Description,
                Amount = request.Request.Amount
            };

            var created = await _repo.CreateCouponAsync(coupon);
            if (!created) return null;

            var created_coupon = await _repo.GetCouponByProductIdAsync(coupon.ProductId);
            return new CouponDto
            {
                Id = created_coupon.Id,
                ProductId = created_coupon.ProductId,
                ProductName = created_coupon.ProductName,
                Description = created_coupon.Description,
                Amount = created_coupon.Amount,
                CreatedAt = created_coupon.CreatedAt,
                UpdatedAt = created_coupon.UpdatedAt
            };
        }
    }
}
