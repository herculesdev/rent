using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Events.MotorbikeUpdated;

public class MotorbikeUpdatedEventHandler(IMotorbikeRepository motorbikeRepository, ILogger<MotorbikeUpdatedEventHandler> logger) : IRequestHandler<MotorbikeUpdatedEvent, Result>
{
    public async Task<Result> Handle(MotorbikeUpdatedEvent evt, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Detecting update type");
        switch (evt.UpdateType)
        {
            case MotorbikeUpdateType.Deleted:
            {
                logger.LogInformation("Detected: deletion");
                logger.LogInformation("Retrieving from repository");
                var motorbike = await motorbikeRepository.GetById(evt.Id, cancellationToken);
                if (motorbike is null)
                    return Result.Failure("Motorbike not found");
                
                logger.LogInformation("Deleting");
                await motorbikeRepository.Delete(motorbike, cancellationToken);
                return Result.Success();
            }
            case MotorbikeUpdateType.Updated:
            {
                logger.LogInformation("Detected: Update");
                logger.LogInformation("Retrieving from repository");
                var motorbike = await motorbikeRepository.GetById(evt.Id, cancellationToken);
                if (motorbike is null)
                    return Result.Failure("Motorbike not found");
                
                logger.LogInformation("Updating fields");
                motorbike.LicensePlate = evt.LicensePlate;
                motorbike.ManufactureYear = evt.ManufactureYear;
                motorbike.ModelName = evt.ModelName;
                
                logger.LogInformation("Saving into repository");
                await motorbikeRepository.Update(motorbike, cancellationToken);
                return Result.Success();
            }
            case MotorbikeUpdateType.Created:
            {
                logger.LogInformation("Detected: Creation");
                logger.LogInformation("Building entity");
                
                var motorbike = new Motorbike();
                motorbike.Id = evt.Id;
                motorbike.LicensePlate = evt.LicensePlate;
                motorbike.ManufactureYear = evt.ManufactureYear;
                motorbike.ModelName = evt.ModelName;
                
                logger.LogInformation("Saving into repository");
                await motorbikeRepository.Add(motorbike, cancellationToken);

                if (evt.ManufactureYear == 2024)
                {
                    logger.LogInformation("Notifying: Motorbike {plate} is from {year}!", evt.LicensePlate, evt.ManufactureYear);
                    await motorbikeRepository.AddInto2024Collection(motorbike, cancellationToken);
                }

                return Result.Success();
            }
            default:
                return Result.Success();
        }
    }
}