using Microsoft.AspNetCore.Mvc;
using Basket.Repositories;
using Basket.Entities;
using Basket.Dtos;
using MediatR;
using Basket.Features.Basket.Queries;
using Basket.Features.Basket.Commands;

namespace Basket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repo;
        private readonly IMediator _mediator;

        public BasketController(IBasketRepository repo, IMediator mediator) => (_repo, _mediator) = (repo, mediator);

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCartDto>> Get(string id)
        {
            var dto = await _mediator.Send(new GetBasketQuery(id));
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> Update([FromBody] ShoppingCartDto dto)
        {
            var updatedDto = await _mediator.Send(new UpdateBasketCommand(dto));
            if (updatedDto == null) return BadRequest();
            return Ok(updatedDto);
        }

        [HttpPost("add/{id}")]
        public async Task<ActionResult<ShoppingCartDto>> AddItem(string id, [FromBody] ShoppingCartItemDto item)
        {
            var added = await _mediator.Send(new Features.Basket.Commands.AddItemCommand(id, item));
            if (added == null) return BadRequest();
            return Ok(added);
        }

        [HttpPost("remove/{id}/{productId}")]
        public async Task<ActionResult<ShoppingCartDto>> RemoveItem(string id, string productId)
        {
            var removed = await _mediator.Send(new Features.Basket.Commands.RemoveItemCommand(id, productId));
            if (removed == null) return NotFound();
            return Ok(removed);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutDto checkout)
        {
            var ok = await _mediator.Send(new Features.Basket.Commands.CheckoutCommand(checkout));
            if (!ok) return BadRequest();
            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var removed = await _mediator.Send(new DeleteBasketCommand(id));
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}
