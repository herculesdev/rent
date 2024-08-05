using MediatR;
using Rent.Renter.Core.Features.Rental.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.GetById;

public record GetRentalByIdQuery(Guid Id) : IRequest<Result<RentalResponse>>;