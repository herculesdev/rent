using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Messaging;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.Update;

public class UpdateMotorbikeCommandHandler(IMotorbikeRepository motorbikeRepository, IMessageDispatcher messageDispatcher, ILogger<UpdateMotorbikeCommandHandler> logger) : IRequestHandler<UpdateMotorbikeCommand, Result<MotorbikeResponse>>
{
    public async Task<Result<MotorbikeResponse>> Handle(UpdateMotorbikeCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating");
        var validation = await new UpdateMotorbikeCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            return Result.Failure(validation.Errors);
        
        logger.LogInformation("Retrieving motorbike");
        var motorbike = await motorbikeRepository.GetById(command.Id, cancellationToken);
        if (motorbike is null)
            return Result.Failure("Motorbike not found");

        if (!command.LicensePlate.Equals(motorbike.LicensePlate, StringComparison.CurrentCultureIgnoreCase))
        {
            logger.LogInformation("Detected license plate update. Checking if it already exists");
            var motorbikeGotByLicensePlate = await motorbikeRepository.GetByLicensePlate(command.LicensePlate, cancellationToken);

            if (motorbikeGotByLicensePlate is not null)
                return Result.Failure(nameof(command.LicensePlate), "License Plate already exists");
        }

        logger.LogInformation("Setting data and saving into repository");
        motorbike.ManufactureYear = command.ManufactureYear;
        motorbike.ModelName = command.ModelName;
        motorbike.LicensePlate = command.LicensePlate;
        await motorbikeRepository.Update(motorbike, cancellationToken);
        
        logger.LogInformation("Publishing event to stream and returning");
        var response = MotorbikeResponse.From(motorbike);
        messageDispatcher.PublishToStream("motorbike-updates-stream", MotorbikeUpdatedDispatchMessage.FromUpdated(response));
        
        return Result.Success(response);
    }
}