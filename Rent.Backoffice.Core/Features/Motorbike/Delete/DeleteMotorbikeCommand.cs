using MediatR;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.Delete;

public record DeleteMotorbikeCommand(Guid Id) : IRequest<Result>;