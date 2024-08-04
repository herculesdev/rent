using MediatR;
using Rent.Backoffice.Core.Features.Motorbike.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Backoffice.Core.Features.Motorbike.Update;

public record UpdateMotorbikeCommand(Guid Id, int ManufactureYear, string ModelName, string LicensePlate) : IRequest<Result<MotorbikeResponse>>;