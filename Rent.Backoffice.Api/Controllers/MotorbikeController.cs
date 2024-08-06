using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rent.Backoffice.Core.Features.Motorbike.Create;
using Rent.Backoffice.Core.Features.Motorbike.Delete;
using Rent.Backoffice.Core.Features.Motorbike.GetById;
using Rent.Backoffice.Core.Features.Motorbike.GetPaged;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Backoffice.Core.Features.Motorbike.Update;
using Rent.Shared.Library.Extensions;
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
        return (await sender.Send(new GetMotorbikeByIdQuery(id))).OkOrNotFound();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MotorbikeResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult>GetPaged([FromQuery]GetMotorbikesPagedQuery query)
    {
        return (await sender.Send(query)).OkOrBadRequest();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateMotorbikeCommand command)
    {
        return (await sender.Send(command)).CreatedOrBadRequest();
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(MotorbikeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateMotorbikeCommand command)
    {
        return (await sender.Send(command)).OkOrBadRequest();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult>Delete(Guid id)
    {
        return (await sender.Send(new DeleteMotorbikeCommand(id))).NoContentOrNotFound();
    }
}