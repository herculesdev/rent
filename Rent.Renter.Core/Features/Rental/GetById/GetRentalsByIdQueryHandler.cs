using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.Rental.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.Rental.GetById;

public class GetRentalByIdQueryHandler(IRentalRepository rentalRepository, ILogger<GetRentalByIdQueryHandler> logger) : IRequestHandler<GetRentalByIdQuery, Result<RentalResponse>>
{
    public async Task<Result<RentalResponse>> Handle(GetRentalByIdQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving");
        var rental = await rentalRepository.GetById(query.Id, cancellationToken);
        if (rental is null)
            return Result.Failure("Rental not found");
        
        logger.LogInformation("Building response and returning");
        return Result.Success(RentalResponse.From(rental));
    }
}