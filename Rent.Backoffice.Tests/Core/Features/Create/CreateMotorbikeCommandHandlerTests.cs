using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.Create;
using Rent.Shared.Library.Messaging;

namespace Rent.Backoffice.Tests.Core.Features.Create;

public class CreateMotorbikeCommandHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly ILogger<CreateMotorbikeCommandHandler> _logger;
    private readonly CreateMotorbikeCommandHandler _createMotorbikeCommandHandler;
    private readonly CreateMotorbikeCommand _createMotorbikeCommandSample = new(2024, "Honda Titan 160", "FMK0B49");

    public CreateMotorbikeCommandHandlerTests()
    {
        _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
        _messageDispatcher = Substitute.For<IMessageDispatcher>();
        _logger = Substitute.For<ILogger<CreateMotorbikeCommandHandler>>();
        
        _createMotorbikeCommandHandler = new CreateMotorbikeCommandHandler(_motorbikeRepository, _messageDispatcher, _logger);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenCommandIsInvalid()
    {
        var invalidCreateMotorbikeCommand = new CreateMotorbikeCommand(ManufactureYear: 0, ModelName: "", LicensePlate: "0000000");

        var result = await _createMotorbikeCommandHandler.Handle(invalidCreateMotorbikeCommand);
        
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_LicensePlateAlreadyExistsInTheDatabase()
    {
        var existentLicensePlate = "PBA4J09";
        var validCreateMotorbikeCommand = _createMotorbikeCommandSample with { LicensePlate = existentLicensePlate };
        _motorbikeRepository.GetByLicensePlate(existentLicensePlate).Returns(new Motorbike());
        
        var expectedErrorCount = 1;
        var expectedErrorMessage = "License Plate already exists in the database";
        
        var result = await _createMotorbikeCommandHandler.Handle(validCreateMotorbikeCommand);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        _motorbikeRepository.GetByLicensePlate(_createMotorbikeCommandSample.LicensePlate).ReturnsNull();
        var expectedRepositoryAddCalls = 1;
        var expectedDispatcherPublishCalls = 1;
        
        var result = await _createMotorbikeCommandHandler.Handle(_createMotorbikeCommandSample);
        
        _ = _motorbikeRepository.Received(expectedRepositoryAddCalls).Add(Arg.Any<Motorbike>());
        _messageDispatcher.Received(expectedDispatcherPublishCalls).PublishToStream(Arg.Any<string>(), Arg.Any<object>());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(_createMotorbikeCommandSample.LicensePlate, result.Data.LicensePlate);
    }
    
    

    
}