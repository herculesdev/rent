using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.GetById;

public class GetMotorbikeByIdQueryHandler(
    IMotorbikeRepository motorbikeRepository,
    ILogger<GetMotorbikeByIdQueryHandler> logger) : IRequestHandler<GetMotorbikeByIdQuery, Result<MotorbikeResponse>>
{
    public async Task<Result<MotorbikeResponse>> Handle(GetMotorbikeByIdQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving motorbike");
        var motorbike = await motorbikeRepository.GetById(query.Id, cancellationToken);
        if (motorbike is null)
            return Result.Failure("Motorbike not found");
        
        logger.LogInformation("Building response");
        var response = MotorbikeResponse.From(motorbike);

        return Result.Success(response);
    }
}