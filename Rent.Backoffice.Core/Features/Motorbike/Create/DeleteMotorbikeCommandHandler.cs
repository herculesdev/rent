using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Messaging;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.Create;

public class CreateMotorbikeCommandHandler(IMotorbikeRepository motorbikeRepository, IMessageDispatcher messageDispatcher, ILogger<CreateMotorbikeCommandHandler> logger) : IRequestHandler<CreateMotorbikeCommand, Result<MotorbikeResponse>>
{
    private const string MotorbikeStreamName = "motorbike-updates-stream";
    public async Task<Result<MotorbikeResponse>> Handle(CreateMotorbikeCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Validating");
        var validation = await new CreateMotorbikeCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            return Result.Failure(validation.Errors);

        var motorbikeGotByLicensePlate = await motorbikeRepository.GetByLicensePlate(command.LicensePlate.ToUpper(), cancellationToken);
        if (motorbikeGotByLicensePlate is not null)
            return Result.Failure(nameof(command.LicensePlate), "License Plate already exists in the database");
        
        logger.LogInformation("Building entity and adding into repository");
        var motorbike = new Entities.Motorbike(command.ManufactureYear, command.ModelName, command.LicensePlate.ToUpper());
        await motorbikeRepository.Add(motorbike, cancellationToken);
        
        logger.LogInformation("Building response");
        var response = MotorbikeResponse.From(motorbike);
        
        logger.LogInformation("Publishing event to stream and returning");
        messageDispatcher.PublishToStream(MotorbikeStreamName, MotorbikeUpdatedDispatchMessage.FromCreated(response));
        
        return Result.Success(response);
    }
}