using MediatR;
using Discount.Dtos;
using Discount.Repositories;

namespace Discount.Features.Coupons.Queries
{
    public sealed class GetCouponByProductIdQuery : IRequest<CouponDto>
    {
        public string ProductId { get; }
        public GetCouponByProductIdQuery(string productId) => ProductId = productId;
    }

    public sealed class GetCouponByProductIdQueryHandler : IRequestHandler<GetCouponByProductIdQuery, CouponDto>
    {
        private readonly IDiscountRepository _repo;
        public GetCouponByProductIdQueryHandler(IDiscountRepository repo) => _repo = repo;

        public async Task<CouponDto> Handle(GetCouponByProductIdQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _repo.GetCouponByProductIdAsync(request.ProductId);
            if (coupon == null) return null;
            return new CouponDto
            {
                Id = coupon.Id,
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
                CreatedAt = coupon.CreatedAt,
                UpdatedAt = coupon.UpdatedAt
            };
        }
    }
}
