using MediatR;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.GetPaged;

public record GetMotorbikesPagedQuery(string? SearchString, int Page = 1, int PageSize = 10) : IRequest<PagedResult<MotorbikeResponse>>
{
    public int GetSkip() => (Page - 1) * PageSize;
}