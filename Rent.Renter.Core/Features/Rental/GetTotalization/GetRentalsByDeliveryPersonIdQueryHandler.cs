using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.ValueObjects;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.GetTotalization;

public class GetRentalTotalizationQueryHandler(IRentalRepository rentalRepository, ILogger<GetRentalTotalizationQueryHandler> logger) : IRequestHandler<GetRentalTotalizationQuery, Result<RentalPriceTotalization>>
{
    public async Task<Result<RentalPriceTotalization>> Handle(GetRentalTotalizationQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving");
        var rental = await rentalRepository.GetById(query.Id, cancellationToken);
        if (rental is null)
            return Result.Failure("Rental not found");
        
        logger.LogInformation("Building totalization and returning");
        var returnDate = query.ReturnDate ?? (rental.EffectiveEndDateUtc ?? rental.EstimatedEndDateUtc);
        return Result.Success(rental.GetTotalization(returnDate));
    }
}