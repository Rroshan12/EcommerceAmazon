using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Dtos;
using Catalog.Specifications;
using Catalog.Features.Products.Queries;
using Catalog.Features.Products.Commands;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<PaginationDto<ProductDto>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string brandId = null, [FromQuery] string typeId = null, [FromQuery] string search = null)
        {
            var spec = new CatalogSpecParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                BrandId = brandId,
                TypeId = typeId,
                Search = search
            };

            var result = await _mediator.Send(new GetProductsQuery(spec));
            return Ok(result);
        }

        // GET api/products/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductDto>> GetById(string id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest req)
        {
            var created = await _mediator.Send(new CreateProductCommand(req));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/products/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ProductDto>> Update(string id, [FromBody] UpdateProductRequest req)
        {
            if (req == null) return BadRequest();
            req.Id = id;
            var updated = await _mediator.Send(new UpdateProductCommand(req));
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE api/products/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<ProductDto>> Delete(string id)
        {
            var deleted = await _mediator.Send(new DeleteProductCommand(id));
            if (deleted == null) return NotFound();
            return Ok(deleted);
        }
    }
}
