using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Features.Rental.Create;

namespace Rent.Renter.Tests.Core.Rental.Create;

public class CreateRentalCommandHandlerTests
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository = Substitute.For<IDeliveryPersonRepository>();
    private readonly IRentalRepository _rentalRepository = Substitute.For<IRentalRepository>();
    private readonly IMotorbikeRepository _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
    private readonly ILogger<CreateRentalCommandHandler> _logger = Substitute.For<ILogger<CreateRentalCommandHandler>>();
    private readonly CreateRentalCommandHandler _createRentalCommandHandler;

    private readonly CreateRentalCommand _createRentalCommand = new CreateRentalCommand(Guid.NewGuid(), Plan.AvailablePlans.FirstOrDefault()!.Id, Guid.NewGuid());
    
    public CreateRentalCommandHandlerTests()
    {
        _createRentalCommandHandler = new CreateRentalCommandHandler(_deliveryPersonRepository, _rentalRepository, _motorbikeRepository, _logger);
    }

    [Fact]
    public async Task Handl_ShouldFail_WhenDeliveryPersonDoesNotExist()
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Delivery Person not found";
        _deliveryPersonRepository.GetById(_createRentalCommand.DeliveryPersonId).ReturnsNull();

        var result = await _createRentalCommandHandler.Handle(_createRentalCommand);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handl_ShouldFail_WhenPlanDoesNotExist()
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Plan not found";
        var command = _createRentalCommand with { PlanId = Guid.NewGuid() };
        _deliveryPersonRepository.GetById(_createRentalCommand.DeliveryPersonId).Returns(new DeliveryPerson());

        var result = await _createRentalCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handl_ShouldFail_WhenMotorbikeDoesNotExist()
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Motorbike not found";
        var command = _createRentalCommand;
        _deliveryPersonRepository.GetById(_createRentalCommand.DeliveryPersonId).Returns(new DeliveryPerson());
        _motorbikeRepository.GetById(_createRentalCommand.MotorbikeId).ReturnsNull();

        var result = await _createRentalCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handl_ShouldFail_WhenMotorbikeIsRented()
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Motorbike is already rented";
        var command = _createRentalCommand;
        _deliveryPersonRepository.GetById(_createRentalCommand.DeliveryPersonId).Returns(new DeliveryPerson());
        _motorbikeRepository.GetById(_createRentalCommand.MotorbikeId).Returns(new Motorbike() { IsRented = true});

        var result = await _createRentalCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var expectedRentalRepositoryAdd = 1;
        var expectedMotorbikeRepositoryAdd = 1;
        var command = _createRentalCommand;
        _deliveryPersonRepository.GetById(_createRentalCommand.DeliveryPersonId).Returns(new DeliveryPerson());
        _motorbikeRepository.GetById(_createRentalCommand.MotorbikeId).Returns(new Motorbike());

        var result = await _createRentalCommandHandler.Handle(command);
        
        Assert.True(result.IsSuccess);
        _ = _rentalRepository.Received(expectedRentalRepositoryAdd).Add(Arg.Any<Renter.Core.Entities.Rental>());
        _ = _motorbikeRepository.Received(expectedMotorbikeRepositoryAdd).Update(Arg.Any<Motorbike>());
    }
}