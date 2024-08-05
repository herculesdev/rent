using MediatR;
using Rent.Renter.Core.Features.Rental.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.Create;

public record CreateRentalCommand(Guid DeliveryPersonId, Guid PlanId, Guid MotorbikeId) : IRequest<Result<RentalResponse>>;