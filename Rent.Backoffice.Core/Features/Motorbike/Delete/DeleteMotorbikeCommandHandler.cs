using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Messaging;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.Delete;

public class DeleteMotorbikeCommandHandler(IMotorbikeRepository motorbikeRepository, IMessageDispatcher messageDispatcher, ILogger<DeleteMotorbikeCommandHandler> logger) : IRequestHandler<DeleteMotorbikeCommand, Result>
{
    public async Task<Result> Handle(DeleteMotorbikeCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving motorbike");
        var motorbike = await motorbikeRepository.GetById(command.Id, cancellationToken);
        if (motorbike is null)
            return Result.Failure("Motorbike not found");
        
        logger.LogInformation("Deleting motorbike from repository");
        await motorbikeRepository.Delete(motorbike, cancellationToken);
        
        logger.LogInformation("Publishing event to stream and returning");
        var response = MotorbikeResponse.From(motorbike);
        messageDispatcher.PublishToStream("motorbike-updates-stream", MotorbikeUpdatedDispatchMessage.FromDeleted(response));

        return Result.Success();
    }
}