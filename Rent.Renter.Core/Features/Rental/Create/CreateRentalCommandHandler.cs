using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Features.Rental.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.Create;

public class CreateRentalCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, IRentalRepository rentalRepository, IMotorbikeRepository motorbikeRepository, ILogger<CreateRentalCommandHandler> logger) : IRequestHandler<CreateRentalCommand, Result<RentalResponse>>
{
    public async Task<Result<RentalResponse>> Handle(CreateRentalCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating");
        var deliveryPerson = await deliveryPersonRepository.GetById(command.DeliveryPersonId, cancellationToken);
        if (deliveryPerson is null)
            return Result.Failure(nameof(command.DeliveryPersonId), "Delivery Person not found");
        
        var plan = Plan.AvailablePlans.FirstOrDefault(x => x.Id == command.PlanId);
        if (plan is null)
            return Result.Failure(nameof(command.PlanId), "Plan not found");

        var motorbike = await motorbikeRepository.GetById(command.MotorbikeId, cancellationToken);
        if(motorbike is null)
            return Result.Failure(nameof(command.MotorbikeId), "Motorbike not found");
        
        if(motorbike.IsRented)
            return Result.Failure(nameof(command.MotorbikeId), "Motorbike is already rented");
        
        logger.LogInformation("Building rental and adding into repository");
        var rental = Entities.Rental.Create(deliveryPerson, plan, motorbike.Id);
        motorbike.IsRented = true;
        await rentalRepository.Add(rental, cancellationToken);
        await motorbikeRepository.Update(motorbike, cancellationToken);
        
        logger.LogInformation("Building response and returning");
        var response = RentalResponse.From(rental);
        return Result.Success(response);
    }
}