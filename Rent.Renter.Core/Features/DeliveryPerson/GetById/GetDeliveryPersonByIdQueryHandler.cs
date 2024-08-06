using MediatR;
using Microsoft.Extensions.Logging;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.Shared;
using Rent.Shared.Library.Results;

namespace Rent.Renter.Core.Features.DeliveryPerson.GetById;

public class GetDeliveryPersonByIdQueryHandler(IDeliveryPersonRepository deliveryPersonRepository, ILogger<GetDeliveryPersonByIdQueryHandler> logger) : IRequestHandler<GetDeliveryPersonByIdQuery, Result<DeliveryPersonResponse>>
{
    public async Task<Result<DeliveryPersonResponse>> Handle(GetDeliveryPersonByIdQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving");
        var deliveryPerson = await deliveryPersonRepository.GetById(query.Id, cancellationToken);
        if (deliveryPerson is null)
            return Result.Failure("Delivery Person not found");
        
        logger.LogInformation("Building response and returning");
        return Result.Success(DeliveryPersonResponse.From(deliveryPerson));
    }
}