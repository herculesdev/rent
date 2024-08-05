using MediatR;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.DeliveryPerson.GetById;

public record GetDeliveryPersonByIdQuery(Guid Id) : IRequest<Result<DeliveryPersonResponse>>;