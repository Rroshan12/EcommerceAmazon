using Discount.Dtos;
using Discount.Entities;
using Discount.Features.Coupons.Commands;
using Discount.Features.Coupons.Queries;
using Discount.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Services
{
    public class DiscountGrpcService : DiscountService.DiscountServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountGrpcService> _logger;

        public DiscountGrpcService(IMediator mediator, ILogger<DiscountGrpcService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<CouponReply> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetDiscount called for ProductId: {ProductId}", request.ProductId);

            var coupon = await _mediator.Send(new GetCouponByProductIdQuery(request.ProductId));
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }

            return MapCouponToCouponReply(coupon);
        }

        public override async Task<CouponReply> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("CreateDiscount called for ProductId: {ProductId}", request.ProductId);

            var createRequest = new CreateCouponRequest
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Description = request.Description,
                Amount = request.Amount
            };

            var coupon = await _mediator.Send(new CreateCouponCommand(createRequest));
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to create coupon"));
            }

            return MapCouponToCouponReply(coupon);
        }

        public override async Task<CouponReply> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("UpdateDiscount called for Id: {Id}", request.Id);

            var updateRequest = new UpdateCouponRequest
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Description = request.Description,
                Amount = request.Amount
            };

            var coupon = await _mediator.Send(new UpdateCouponCommand(updateRequest));
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }

            return MapCouponToCouponReply(coupon);
        }

        public override async Task<DeleteDiscountReply> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DeleteDiscount called for Id: {Id}", request.Id);

            var success = await _mediator.Send(new DeleteCouponCommand(request.Id));
            if (!success)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }

            return new DeleteDiscountReply { Success = true };
        }

        private CouponReply MapCouponToCouponReply(CouponDto coupon)
        {
            return new CouponReply
            {
                Id = coupon.Id,
                ProductId = coupon.ProductId,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
                CreatedAt = coupon.CreatedAt.ToString("O"),
                UpdatedAt = coupon.UpdatedAt.ToString("O")
            };
        }
    }
}
