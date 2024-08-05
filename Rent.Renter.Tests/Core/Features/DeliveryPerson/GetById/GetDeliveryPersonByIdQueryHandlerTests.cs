using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.GetById;

namespace Rent.Renter.Tests.Core.Features.DeliveryPerson.GetById;

public class GetDeliveryPersonByIdQueryHandlerTests
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository = Substitute.For<IDeliveryPersonRepository>();
    private readonly ILogger<GetDeliveryPersonByIdQueryHandler> _logger = Substitute.For<ILogger<GetDeliveryPersonByIdQueryHandler>>();
    private readonly GetDeliveryPersonByIdQueryHandler _getDeliveryPersonByIdQueryHandler;
    
    public GetDeliveryPersonByIdQueryHandlerTests()
    {
        _getDeliveryPersonByIdQueryHandler = new GetDeliveryPersonByIdQueryHandler(_deliveryPersonRepository, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDeliveryPersonDoesNotExist()
    {
        var query = new GetDeliveryPersonByIdQuery(Guid.NewGuid());
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Delivery Person not found";
        _deliveryPersonRepository.GetById(query.Id).ReturnsNull();
            
        var result = await _getDeliveryPersonByIdQueryHandler.Handle(query);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var query = new GetDeliveryPersonByIdQuery(Guid.NewGuid());
        _deliveryPersonRepository.GetById(query.Id).Returns(new Renter.Core.Entities.DeliveryPerson { Id = query.Id });
            
        var result = await _getDeliveryPersonByIdQueryHandler.Handle(query);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(query.Id, result.Data?.Id);
    }
}