using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Renter.Core.Features.DeliveryPerson.Create;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;

namespace Rent.Renter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryPersonController(ISender sender) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        await Task.Delay(10);
        return Ok("Not implemented yet");
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryPersonResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateDeliveryPersonCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(Get), new{ Id = result.Data!.Id }, result.Data);
    }
    
    /*[HttpPatch]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateMotorbikeCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);
        
        return Ok(result.Data);
    }*/
}