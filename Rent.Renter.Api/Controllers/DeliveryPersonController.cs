using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Renter.Core.Features.DeliveryPerson.Create;
using Rent.Renter.Core.Features.DeliveryPerson.GetById;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Renter.Core.Features.DeliveryPerson.Update;
using Rent.Shared.Library.Extensions;

namespace Rent.Renter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryPersonController(ISender sender) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return (await sender.Send(new GetDeliveryPersonByIdQuery(id))).OkOrNotFound();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryPersonResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateDeliveryPersonCommand command)
    {
        return (await sender.Send(command)).CreatedOrBadRequest();
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(DeliveryPersonResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateDeliveryPersonCommand command)
    {
        return (await sender.Send(command)).OkOrBadRequest();
    }
}