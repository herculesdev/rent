using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Entities;
using Rent.Renter.Core.Events.MotorbikeUpdated;

namespace Rent.Renter.Tests.Core.Events;

public class MotorbikeUpdatedEventHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
    private readonly ILogger<MotorbikeUpdatedEventHandler> _logger = Substitute.For<ILogger<MotorbikeUpdatedEventHandler>>();
    private readonly MotorbikeUpdatedEvent _motorbikeUpdatedEventSample = new MotorbikeUpdatedEvent(Guid.NewGuid(), 2024, "Honda Titan 160", "PBA4J09", MotorbikeUpdateType.Created);
    private readonly MotorbikeUpdatedEventHandler _motorbikeUpdatedEventHandler;
        
    public MotorbikeUpdatedEventHandlerTests()
    {
        _motorbikeUpdatedEventHandler = new MotorbikeUpdatedEventHandler(_motorbikeRepository, _logger);
    }

    [Theory]
    [InlineData(MotorbikeUpdateType.Updated)]
    [InlineData(MotorbikeUpdateType.Deleted)]
    public async Task Handle_ShouldFail_WhenIsUpdateOrDeleteAndMotorbikeDoesNotExist(MotorbikeUpdateType updateType)
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Motorbike not found";
        var evt = _motorbikeUpdatedEventSample with { UpdateType = updateType };
        _motorbikeRepository.GetById(evt.Id).ReturnsNull();

        var result = await _motorbikeUpdatedEventHandler.Handle(evt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceedForDeletion()
    {
        var expectedRepositoryDeleteCalls = 1;
        var motorbikeFromRepository = new Motorbike();
        var evt = _motorbikeUpdatedEventSample with { UpdateType = MotorbikeUpdateType.Deleted };
        _motorbikeRepository.GetById(evt.Id).Returns(motorbikeFromRepository);

        var result = await _motorbikeUpdatedEventHandler.Handle(evt);

        _ = _motorbikeRepository.Received(expectedRepositoryDeleteCalls).Delete(motorbikeFromRepository);
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceedForUpdate()
    {
        var expectedRepositoryUpdateCalls = 1;
        var motorbikeFromRepository = new Motorbike();
        var evt = _motorbikeUpdatedEventSample with { UpdateType = MotorbikeUpdateType.Updated };
        _motorbikeRepository.GetById(evt.Id).Returns(motorbikeFromRepository);

        var result = await _motorbikeUpdatedEventHandler.Handle(evt);

        _ = _motorbikeRepository.Received(expectedRepositoryUpdateCalls).Update(motorbikeFromRepository);
        Assert.Equal(evt.LicensePlate, motorbikeFromRepository.LicensePlate);
        Assert.Equal(evt.ManufactureYear, motorbikeFromRepository.ManufactureYear);
        Assert.Equal(evt.ModelName, motorbikeFromRepository.ModelName);
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceedForCreation()
    {
        var expectedRepositoryAddCalls = 1;
        var evt = _motorbikeUpdatedEventSample with { UpdateType = MotorbikeUpdateType.Created };
        Motorbike? addedMotorbike = null;
        _motorbikeRepository.When(x => x.Add(Arg.Any<Motorbike>())).Do(callInfo => addedMotorbike = callInfo.ArgAt<Motorbike>(0));
        var result = await _motorbikeUpdatedEventHandler.Handle(evt);

        _ = _motorbikeRepository.Received(expectedRepositoryAddCalls).Add(Arg.Any<Motorbike>());
        Assert.NotNull(addedMotorbike);
        Assert.Equal(evt.LicensePlate, addedMotorbike.LicensePlate);
        Assert.Equal(evt.ManufactureYear, addedMotorbike.ManufactureYear);
        Assert.Equal(evt.ModelName, addedMotorbike.ModelName);
        Assert.True(result.IsSuccess);
    }
}