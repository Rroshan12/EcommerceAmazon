using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Dtos;
using Catalog.Features.Types.Queries;
using Catalog.Features.Types.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TypesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> Get()
        {
            var items = await _mediator.Send(new GetTypesQuery());
            return Ok(items);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductTypeDto>> GetById(string id)
        {
            var item = await _mediator.Send(new GetTypeByIdQuery(id));
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ProductTypeDto>> Create([FromBody] CreateTypeRequest req)
        {
            var created = await _mediator.Send(new CreateTypeCommand(req));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ProductTypeDto>> Update(string id, [FromBody] UpdateTypeRequest req)
        {
            if (req == null) return BadRequest();
            req.Id = id;
            var updated = await _mediator.Send(new UpdateTypeCommand(req));
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
