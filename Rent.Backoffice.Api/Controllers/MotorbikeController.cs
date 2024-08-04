using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Backoffice.Core.Features.Motorbike.Create;
using Rent.Backoffice.Core.Features.Motorbike.Delete;
using Rent.Backoffice.Core.Features.Motorbike.GetById;
using Rent.Backoffice.Core.Features.Motorbike.GetPaged;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Backoffice.Core.Features.Motorbike.Update;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotorbikeController(ISender sender) : Controller
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult>GetById(Guid id)
    {
        var result = await sender.Send(new GetMotorbikeByIdQuery(id));

        if (result.IsFailure)
            return NotFound();

        return Ok(result.Data);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MotorbikeResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult>GetPaged([FromQuery]GetMotorbikesPagedQuery query)
    {
        var result = await sender.Send(query);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(new { result.Items, result.Meta});
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateMotorbikeCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(GetById), new {id = result.Data!.Id}, result.Data);
    }
    
    [HttpPatch]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateMotorbikeCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);
        
        return Ok(result.Data);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult>Delete(Guid id)
    {
        var result = await sender.Send(new DeleteMotorbikeCommand(id));

        if (result.IsFailure)
            return NotFound(result.Errors);

        return NoContent();
    }
}