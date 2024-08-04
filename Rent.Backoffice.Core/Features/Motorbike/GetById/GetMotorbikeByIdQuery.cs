using MediatR;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.GetById;

public record GetMotorbikeByIdQuery(Guid Id) : IRequest<Result<MotorbikeResponse>>;