using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Features.Rental.GetTotalization;

namespace Rent.Renter.Tests.Core.Rental.GetTotalization;

public class GetRentalTotalizationHandlerTests
{
    private readonly IRentalRepository _rentalRepository = Substitute.For<IRentalRepository>();
    private readonly ILogger<GetRentalTotalizationQueryHandler> _logger = Substitute.For<ILogger<GetRentalTotalizationQueryHandler>>();
    private readonly GetRentalTotalizationQueryHandler _getRentalTotalizationQueryHandler;
    
    public GetRentalTotalizationHandlerTests()
    {
        _getRentalTotalizationQueryHandler = new GetRentalTotalizationQueryHandler(_rentalRepository, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenRentalDoesNotExist()
    {
        var query = new GetRentalTotalizationQuery(Guid.NewGuid(), null);
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Rental not found";
        _rentalRepository.GetById(query.Id).ReturnsNull();
            
        var result = await _getRentalTotalizationQueryHandler.Handle(query);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var query = new GetRentalTotalizationQuery(Guid.NewGuid(), null);
        var rental = Renter.Core.Entities.Rental.Create(new DeliveryPerson(), Plan.AvailablePlans.FirstOrDefault()!, Guid.NewGuid());
        rental.Id = query.Id;
        _rentalRepository.GetById(query.Id).Returns(rental);
            
        var result = await _getRentalTotalizationQueryHandler.Handle(query);
        
        Assert.True(result.IsSuccess);
    }
}