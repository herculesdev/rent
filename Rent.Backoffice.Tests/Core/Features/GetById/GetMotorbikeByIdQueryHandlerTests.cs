using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.GetById;

namespace Rent.Backoffice.Tests.Core.Features.GetById;

public class GetMotorbikeByIdQueryHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository;
    private readonly ILogger<GetMotorbikeByIdQueryHandler> _logger;
    private readonly GetMotorbikeByIdQueryHandler _getMotorbikeByIdQueryHandler;
    private readonly GetMotorbikeByIdQuery _getMotorbikeByIdQuerySample = new(Guid.NewGuid());

    public GetMotorbikeByIdQueryHandlerTests()
    {
        _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
        _logger = Substitute.For<ILogger<GetMotorbikeByIdQueryHandler>>();
        
        _getMotorbikeByIdQueryHandler = new GetMotorbikeByIdQueryHandler(_motorbikeRepository, _logger);
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenMotorbikeDoesNotExists()
    {
        var expectedErrorCount = 1;
        var expectedErrorMessage = "Motorbike not found";
        _motorbikeRepository.GetById(_getMotorbikeByIdQuerySample.Id).ReturnsNull();
        
        var result = await _getMotorbikeByIdQueryHandler.Handle(_getMotorbikeByIdQuerySample);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedErrorMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var motorbikeFromRepository = new Motorbike(_getMotorbikeByIdQuerySample.Id, 2024, "Honda Titan 160", "PBA4J09");
        _motorbikeRepository.GetById(_getMotorbikeByIdQuerySample.Id).Returns(motorbikeFromRepository);
        
        var result = await _getMotorbikeByIdQueryHandler.Handle(_getMotorbikeByIdQuerySample);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(motorbikeFromRepository.Id, result.Data.Id);
    }
}