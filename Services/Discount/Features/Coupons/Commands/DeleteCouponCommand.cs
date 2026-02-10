using MediatR;
using Discount.Repositories;

namespace Discount.Features.Coupons.Commands
{
    public sealed class DeleteCouponCommand : IRequest<bool>
    {
        public int Id { get; }
        public DeleteCouponCommand(int id) => Id = id;
    }

    public sealed class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, bool>
    {
        private readonly IDiscountRepository _repo;
        public DeleteCouponCommandHandler(IDiscountRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteCouponAsync(request.Id);
        }
    }
}
