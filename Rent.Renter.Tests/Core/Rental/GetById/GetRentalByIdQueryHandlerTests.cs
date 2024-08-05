using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Features.Rental.GetById;

namespace Rent.Renter.Tests.Core.Rental.GetById;

public class GetRentalByIdQueryHandlerTests
{
    private readonly IRentalRepository _rentalRepository = Substitute.For<IRentalRepository>();
    private readonly ILogger<GetRentalByIdQueryHandler> _logger = Substitute.For<ILogger<GetRentalByIdQueryHandler>>();
    private readonly GetRentalByIdQueryHandler _getRentalByIdQueryHandler;
    
    public GetRentalByIdQueryHandlerTests()
    {
        _getRentalByIdQueryHandler = new GetRentalByIdQueryHandler(_rentalRepository, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenRentalDoesNotExist()
    {
        var query = new GetRentalByIdQuery(Guid.NewGuid());
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Rental not found";
        _rentalRepository.GetById(query.Id).ReturnsNull();
            
        var result = await _getRentalByIdQueryHandler.Handle(query);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var query = new GetRentalByIdQuery(Guid.NewGuid());
        var rental = Renter.Core.Entities.Rental.Create(new DeliveryPerson(), Plan.AvailablePlans.FirstOrDefault()!, Guid.NewGuid());
        rental.Id = query.Id;
        _rentalRepository.GetById(query.Id).Returns(rental);
            
        var result = await _getRentalByIdQueryHandler.Handle(query);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(query.Id, result.Data?.Id);
    }
}