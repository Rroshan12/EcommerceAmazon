using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Dtos;
using Catalog.Features.Brands.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBrandDto>>> Get()
        {
            var items = await _mediator.Send(new GetBrandsQuery());
            return Ok(items);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductBrandDto>> GetById(string id)
        {
            var item = await _mediator.Send(new GetBrandByIdQuery(id));
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
