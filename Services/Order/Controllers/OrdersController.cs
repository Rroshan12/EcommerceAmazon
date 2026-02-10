using Microsoft.AspNetCore.Mvc;
using MediatR;
using Order.Dtos;
using Order.Features.Orders.Queries;
using Order.Features.Orders.Commands;

namespace Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserId(string userId)
        {
            var orders = await _mediator.Send(new GetOrdersByUserIdQuery(userId));
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var created = await _mediator.Send(new CreateOrderCommand(request));
            if (created == null) return BadRequest();
            return CreatedAtAction(nameof(GetOrderById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            if (request == null) return BadRequest();
            request.Id = id;
            var updated = await _mediator.Send(new UpdateOrderCommand(request));
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var deleted = await _mediator.Send(new DeleteOrderCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
