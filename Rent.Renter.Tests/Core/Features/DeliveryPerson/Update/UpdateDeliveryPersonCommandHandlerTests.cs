using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.Update;

namespace Rent.Renter.Tests.Core.Features.DeliveryPerson.Update;

public class UpdateDeliveryPersonCommandHandlerTests
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository = Substitute.For<IDeliveryPersonRepository>();
    private readonly IFileStorage _fileStorage = Substitute.For<IFileStorage>();
    private readonly ILogger<UpdateDeliveryPersonCommandHandler> _logger = Substitute.For<ILogger<UpdateDeliveryPersonCommandHandler>>();
    private readonly UpdateDeliveryPersonCommand _updateDeliveryPersonCommandSample = new(Guid.NewGuid(), "Hércules", "16599855000110", "42997067713", "AB", DateTime.Parse("1997-03-22"), Consts.Base64BmpCnh);
    private readonly UpdateDeliveryPersonCommandHandler _updateDeliveryPersonCommandHandler;
    
    public UpdateDeliveryPersonCommandHandlerTests()
    {
        _updateDeliveryPersonCommandHandler = new UpdateDeliveryPersonCommandHandler(_deliveryPersonRepository, _fileStorage, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommandIsInvalid()
    {
        var command = _updateDeliveryPersonCommandSample with { Name = "", DriverLicenseType = "C" };
        var expectedErrorCount = 2;
        
        var result = await _updateDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenDeliveryPersonDoesNotExist()
    {
        var command = _updateDeliveryPersonCommandSample;
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Delivery person not found";

        _deliveryPersonRepository.GetById(command.Id).ReturnsNull();
        
        var result = await _updateDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenDocumentNumberAlreadyExists()
    {
        var existentDocumentNumber = "75686210000184";
        var command = _updateDeliveryPersonCommandSample with { DocumentNumber = existentDocumentNumber};
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Document number already exists";
        
        _deliveryPersonRepository.GetById(command.Id).Returns(new Renter.Core.Entities.DeliveryPerson());
        _deliveryPersonRepository.GetByDocumentNumber(existentDocumentNumber).Returns(new Renter.Core.Entities.DeliveryPerson());
        
        var result = await _updateDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenDriverLicenseNumberAlreadyExists()
    {
        var existentDriverLicenseNumber = "86300239337";
        var command = _updateDeliveryPersonCommandSample with { DriverLicenseNumber = existentDriverLicenseNumber};
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Driver license number already exists";
        
        _deliveryPersonRepository.GetById(command.Id).Returns(new Renter.Core.Entities.DeliveryPerson());
        _deliveryPersonRepository.GetByDriverLicenseNumber(existentDriverLicenseNumber).Returns(new Renter.Core.Entities.DeliveryPerson());
        
        var result = await _updateDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var command = _updateDeliveryPersonCommandSample;
        var expectedCallsToFileStorageSave = 1;
        var expectedCallsToRepositoryAdd = 1;
        _deliveryPersonRepository.GetById(command.Id).Returns(new Renter.Core.Entities.DeliveryPerson());
        
        var result = await _updateDeliveryPersonCommandHandler.Handle(command);
        
        _fileStorage.Received(expectedCallsToFileStorageSave).Save(Arg.Any<string>(), Arg.Any<byte[]>());
        _ = _deliveryPersonRepository.Received(expectedCallsToRepositoryAdd).Update(Arg.Any<Renter.Core.Entities.DeliveryPerson>());
        Assert.True(result.IsSuccess);
        
    }
}