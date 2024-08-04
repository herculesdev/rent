using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.Update;
using Rent.Shared.Library.Messaging;

namespace Rent.Backoffice.Tests.Core.Features.Update;

public class UpdateMotorbikeCommandHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly ILogger<UpdateMotorbikeCommandHandler> _logger;
    private readonly UpdateMotorbikeCommandHandler _updateMotorbikeCommandHandler;
    private readonly UpdateMotorbikeCommand _updateMotorbikeCommandSample = new(Guid.NewGuid(), 2024, "Honda Titan 160", "FMK0B49");

    public UpdateMotorbikeCommandHandlerTests()
    {
        _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
        _messageDispatcher = Substitute.For<IMessageDispatcher>();
        _logger = Substitute.For<ILogger<UpdateMotorbikeCommandHandler>>();
        
        _updateMotorbikeCommandHandler = new UpdateMotorbikeCommandHandler(_motorbikeRepository, _messageDispatcher, _logger);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenCommandIsInvalid()
    {
        var invalidUpdateMotorbikeCommand = new UpdateMotorbikeCommand(Guid.NewGuid(), ManufactureYear: 0, ModelName: "", LicensePlate: "0000000");

        var result = await _updateMotorbikeCommandHandler.Handle(invalidUpdateMotorbikeCommand);
        
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenMotorbikeDoesNotExist()
    {
        _motorbikeRepository.GetById(_updateMotorbikeCommandSample.Id).ReturnsNull();
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Motorbike not found";
        
        var result = await _updateMotorbikeCommandHandler.Handle(_updateMotorbikeCommandSample);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenLicensePlateAlreadyExistsInTheDatabase()
    {
        var existentLicensePlate = "PBA4J09";
        var validUpdateMotorbikeCommand = _updateMotorbikeCommandSample with { LicensePlate = existentLicensePlate };
        _motorbikeRepository.GetById(_updateMotorbikeCommandSample.Id).Returns(new Motorbike { LicensePlate = "ABC00A0"});
        _motorbikeRepository.GetByLicensePlate(existentLicensePlate).Returns(new Motorbike());
        
        var expectedErrorCount = 1;
        var expectedErrorMessage = "License Plate already exists";
        
        var result = await _updateMotorbikeCommandHandler.Handle(validUpdateMotorbikeCommand);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var motorbikeFromRepository = new Motorbike(_updateMotorbikeCommandSample.Id, 2020, "Model Before Update", "AAA00A0");
        var expectedRepositoryUpdateCalls = 1;
        var expectedDispatcherPublishCalls = 1;
        
        _motorbikeRepository.GetByLicensePlate(_updateMotorbikeCommandSample.LicensePlate).ReturnsNull();
        _motorbikeRepository.GetById(_updateMotorbikeCommandSample.Id).Returns(motorbikeFromRepository);
        
        var result = await _updateMotorbikeCommandHandler.Handle(_updateMotorbikeCommandSample);
        
        _ = _motorbikeRepository.Received(expectedRepositoryUpdateCalls).Update(motorbikeFromRepository);
        _messageDispatcher.Received(expectedDispatcherPublishCalls).PublishToStream(Arg.Any<string>(), Arg.Any<object>());
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(_updateMotorbikeCommandSample.ManufactureYear, motorbikeFromRepository.ManufactureYear);
        Assert.Equal(_updateMotorbikeCommandSample.ModelName, motorbikeFromRepository.ModelName);
        Assert.Equal(_updateMotorbikeCommandSample.LicensePlate, motorbikeFromRepository.LicensePlate);
    }
    
    

    
}