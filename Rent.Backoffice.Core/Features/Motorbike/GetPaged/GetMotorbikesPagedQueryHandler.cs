using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.GetPaged;

public class GetMotorbikesPagedQueryHandler(IMotorbikeRepository motorbikeRepository, ILogger<GetMotorbikesPagedQueryHandler> logger) : IRequestHandler<GetMotorbikesPagedQuery, PagedResult<MotorbikeResponse>>
{
    public async Task<PagedResult<MotorbikeResponse>> Handle(GetMotorbikesPagedQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving motorbikes");
        var pagedMotorbikes = await motorbikeRepository.GetPaged(query, cancellationToken);
        
        logger.LogInformation("Building response and returning");
        var responseMotorbikes = pagedMotorbikes.Select(MotorbikeResponse.From).ToList();
        return new PagedResult<MotorbikeResponse>(responseMotorbikes, pagedMotorbikes.Page, pagedMotorbikes.PerPage, pagedMotorbikes.TotalItemCount);
    }
}