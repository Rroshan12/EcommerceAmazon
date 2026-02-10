using MediatR;
using Discount.Dtos;
using Discount.Repositories;

namespace Discount.Features.Coupons.Queries
{
    public sealed class GetAllCouponsQuery : IRequest<IEnumerable<CouponDto>> { }

    public sealed class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQuery, IEnumerable<CouponDto>>
    {
        private readonly IDiscountRepository _repo;
        public GetAllCouponsQueryHandler(IDiscountRepository repo) => _repo = repo;

        public async Task<IEnumerable<CouponDto>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
        {
            var coupons = await _repo.GetAllCouponsAsync();
            return coupons.Select(c => new CouponDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                Description = c.Description,
                Amount = c.Amount,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }
    }
}
