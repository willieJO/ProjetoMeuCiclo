using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeuCiclo.Application.Commands;
using MeuCiclo.Application.Queries;

namespace MeuCiclo.Api.Controllers;

[ApiController]
[Route("api/ciclos")]
public class CiclosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CiclosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _mediator.Send(new GetCiclosQuery()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCicloCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCicloCommand body)
    {
        await _mediator.Send(body with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCicloCommand(id));
        return NoContent();
    }
}
