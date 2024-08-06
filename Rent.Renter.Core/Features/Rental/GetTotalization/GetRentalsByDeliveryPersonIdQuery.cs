using MediatR;
using Rent.Renter.Core.ValueObjects;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.GetTotalization;

public record GetRentalTotalizationQuery(Guid Id, DateOnly? ReturnDate) : IRequest<Result<RentalPriceTotalization>>;