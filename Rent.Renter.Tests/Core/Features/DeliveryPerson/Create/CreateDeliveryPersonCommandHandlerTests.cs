using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Rent.Renter.Core.Data;
using Rent.Renter.Core.Features.DeliveryPerson.Create;

namespace Rent.Renter.Tests.Core.Features.DeliveryPerson.Create;

public class CreateDeliveryPersonCommandHandlerTests
{
    private readonly IDeliveryPersonRepository _deliveryPersonRepository = Substitute.For<IDeliveryPersonRepository>();
    private readonly IFileStorage _fileStorage = Substitute.For<IFileStorage>();
    private readonly ILogger<CreateDeliveryPersonCommandHandler> _logger = Substitute.For<ILogger<CreateDeliveryPersonCommandHandler>>();
    private readonly CreateDeliveryPersonCommand _createDeliveryPersonCommandSample = new("Hércules", "16599855000110", "42997067713", "AB", DateTime.Parse("1997-03-22"), Consts.Base64BmpCnh);
    private readonly CreateDeliveryPersonCommandHandler _createDeliveryPersonCommandHandler;
    
    public CreateDeliveryPersonCommandHandlerTests()
    {
        _createDeliveryPersonCommandHandler = new CreateDeliveryPersonCommandHandler(_deliveryPersonRepository, _fileStorage, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCommandIsInvalid()
    {
        var command = _createDeliveryPersonCommandSample with { Name = "", DriverLicenseType = "C" };
        var expectedErrorCount = 2;
        
        var result = await _createDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenDocumentNumberAlreadyExists()
    {
        var existentDocumentNumber = "75686210000184";
        var command = _createDeliveryPersonCommandSample with { DocumentNumber = existentDocumentNumber};
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Document number already exists";
        
        _deliveryPersonRepository.GetByDocumentNumber(existentDocumentNumber).Returns(new Renter.Core.Entities.DeliveryPerson());
        
        var result = await _createDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenDriverLicenseNumberAlreadyExists()
    {
        var existentDriverLicenseNumber = "34199190106";
        var command = _createDeliveryPersonCommandSample with { DriverLicenseNumber = existentDriverLicenseNumber};
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Driver license number already exists";
        
        _deliveryPersonRepository.GetByDriverLicenseNumber(existentDriverLicenseNumber).Returns(new Renter.Core.Entities.DeliveryPerson());
        
        var result = await _createDeliveryPersonCommandHandler.Handle(command);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var command = _createDeliveryPersonCommandSample;
        var expectedCallsToFileStorageSave = 1;
        var expectedCallsToRepositoryAdd = 1;
        
        var result = await _createDeliveryPersonCommandHandler.Handle(command);
        
        _fileStorage.Received(expectedCallsToFileStorageSave).Save(Arg.Any<string>(), Arg.Any<byte[]>());
        _ = _deliveryPersonRepository.Received(expectedCallsToRepositoryAdd).Add(Arg.Any<Renter.Core.Entities.DeliveryPerson>());
        Assert.True(result.IsSuccess);
        
    }
}