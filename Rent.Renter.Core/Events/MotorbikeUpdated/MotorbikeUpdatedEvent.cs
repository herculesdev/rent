using MediatR;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Events.MotorbikeUpdated;

public record MotorbikeUpdatedEvent(Guid Id, int ManufactureYear, string ModelName, string LicensePlate, MotorbikeUpdateType UpdateType) : IRequest<Result>;
public enum MotorbikeUpdateType { Created = 1, Updated = 2, Deleted = 3 }