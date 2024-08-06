using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Features.Rental.Create;
using Rent.Renter.Core.Features.Rental.GetById;
using Rent.Renter.Core.Features.Rental.GetTotalization;
using Rent.Renter.Core.Features.Rental.Shared;
using Rent.Shared.Library.Extensions;

namespace Rent.Renter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalController(ISender sender) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return (await sender.Send(new GetRentalByIdQuery(id))).OkOrNotFound();
        /*var result = await sender.Send(new GetRentalByIdQuery(id));

        if (result.IsFailure)
            return NotFound(result.Errors);

        return Ok(result.Data);*/
    }
    
    [HttpGet("{id:guid}/totalization")]
    public async Task<IActionResult> GetTotalizationById(Guid id, [FromQuery]DateTime? returnDate)
    {
        DateOnly? date = returnDate != null ? DateOnly.FromDateTime(returnDate.Value) : null;
        return (await sender.Send(new GetRentalTotalizationQuery(id, date))).OkOrNotFound();
        /*var result = await sender.Send(new GetRentalTotalizationQuery(id, date));

        if (result.IsFailure)
            return NotFound(result.Errors);
        
        return Ok(result.Data);*/
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(RentalResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateRentalCommand command)
    {
        return (await sender.Send(command)).CreatedOrBadRequest();
        /*var result = await sender.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetById), new{ id = result.Data!.Id }, result.Data);*/
    }
    
    [HttpGet("available-plans")]
    public IActionResult GetAvailablePlans()
    {
        return Ok(Plan.AvailablePlans);
    }
}