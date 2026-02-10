using Microsoft.AspNetCore.Mvc;
using MediatR;
using Discount.Dtos;
using Discount.Features.Coupons.Queries;
using Discount.Features.Coupons.Commands;

namespace Discount.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CouponsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<CouponDto>> GetCouponByProductId(string productId)
        {
            var coupon = await _mediator.Send(new GetCouponByProductIdQuery(productId));
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponDto>>> GetAllCoupons()
        {
            var coupons = await _mediator.Send(new GetAllCouponsQuery());
            return Ok(coupons);
        }

        [HttpPost]
        public async Task<ActionResult<CouponDto>> CreateCoupon([FromBody] CreateCouponRequest request)
        {
            var created = await _mediator.Send(new CreateCouponCommand(request));
            if (created == null) return BadRequest();
            return CreatedAtAction(nameof(GetCouponByProductId), new { productId = created.ProductId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CouponDto>> UpdateCoupon(int id, [FromBody] UpdateCouponRequest request)
        {
            if (request == null) return BadRequest();
            request.Id = id;
            var updated = await _mediator.Send(new UpdateCouponCommand(request));
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCoupon(int id)
        {
            var deleted = await _mediator.Send(new DeleteCouponCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
