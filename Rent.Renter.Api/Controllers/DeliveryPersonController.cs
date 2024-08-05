using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Renter.Core.Features.DeliveryPerson.Create;
using Rent.Renter.Core.Features.DeliveryPerson.GetById;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Renter.Core.Features.DeliveryPerson.Update;

namespace Rent.Renter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryPersonController(ISender sender) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await sender.Send(new GetDeliveryPersonByIdQuery(id));

        if (result.IsFailure)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryPersonResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateDeliveryPersonCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);
        
        return CreatedAtAction(nameof(GetById), new{ result.Data!.Id }, result.Data);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(DeliveryPersonResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateDeliveryPersonCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }
}