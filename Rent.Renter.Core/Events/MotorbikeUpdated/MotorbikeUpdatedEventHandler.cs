using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Events.MotorbikeUpdated;

public class MotorbikeUpdatedEventHandler(IMotorbikeRepository motorbikeRepository, ILogger<MotorbikeUpdatedEventHandler> logger) : IRequestHandler<MotorbikeUpdatedEvent, Result>
{
    public async Task<Result> Handle(MotorbikeUpdatedEvent evt, CancellationToken cancellationToken)
    {
        switch (evt.UpdateType)
        {
            case MotorbikeUpdateType.Deleted:
            {
                var motorbike = await motorbikeRepository.GetById(evt.Id, cancellationToken);
                if (motorbike is null)
                    return Result.Failure("Can't delete non existent motorbike");
            
                await motorbikeRepository.Delete(motorbike, cancellationToken);
                return Result.Success();
            }
            case MotorbikeUpdateType.Updated or MotorbikeUpdateType.Deleted:
            {
                var motorbike = await motorbikeRepository.GetById(evt.Id, cancellationToken);
                if (motorbike is null)
                    return Result.Failure("Can't update non existent motorbike");

                motorbike.LicensePlate = evt.LicensePlate;
                motorbike.ManufactureYear = evt.ManufactureYear;
                motorbike.ModelName = evt.ModelName;
                await motorbikeRepository.Update(motorbike, cancellationToken);
                return Result.Success();
            }
            case MotorbikeUpdateType.Created:
            {
                var motorbike = new Motorbike();
                motorbike.Id = evt.Id;
                motorbike.LicensePlate = evt.LicensePlate;
                motorbike.ManufactureYear = evt.ManufactureYear;
                motorbike.ModelName = evt.ModelName;
                await motorbikeRepository.Add(motorbike, cancellationToken);
                return Result.Success();
            }
            default:
                return Result.Success();
        }
    }
}