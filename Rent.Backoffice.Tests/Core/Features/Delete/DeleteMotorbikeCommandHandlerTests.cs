using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.Delete;
using Rent.Shared.Library.Messaging;

namespace Rent.Backoffice.Tests.Core.Features.Delete;

public class DeleteMotorbikeCommandHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly ILogger<DeleteMotorbikeCommandHandler> _logger;
    private readonly DeleteMotorbikeCommandHandler _deleteMotorbikeCommandHandler;
    private readonly DeleteMotorbikeCommand _deleteMotorbikeCommandSample = new(Guid.NewGuid());

    public DeleteMotorbikeCommandHandlerTests()
    {
        _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
        _messageDispatcher = Substitute.For<IMessageDispatcher>();
        _logger = Substitute.For<ILogger<DeleteMotorbikeCommandHandler>>();
        
        _deleteMotorbikeCommandHandler = new DeleteMotorbikeCommandHandler(_motorbikeRepository, _messageDispatcher, _logger);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenMotorbikeDoesNotExists()
    {
        var expectedErrorCount = 1;
        var expectedMessage = "Motorbike not found";
        _motorbikeRepository.GetById(_deleteMotorbikeCommandSample.Id).ReturnsNull();
        
        var result = await _deleteMotorbikeCommandHandler.Handle(_deleteMotorbikeCommandSample);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
        Assert.Equal(expectedMessage, result.Errors.FirstOrDefault()?.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var motorbikeEntityFromRepository = new Motorbike();
        var expectedRepositoryDeleteCalls = 1;
        var expectedDispatcherPublishCalls = 1;
        _motorbikeRepository.GetById(_deleteMotorbikeCommandSample.Id).Returns(motorbikeEntityFromRepository);
        
        var result = await _deleteMotorbikeCommandHandler.Handle(_deleteMotorbikeCommandSample);
        
        await _motorbikeRepository.Received(expectedRepositoryDeleteCalls).Delete(motorbikeEntityFromRepository);
        _messageDispatcher.Received(expectedDispatcherPublishCalls).PublishToStream(Arg.Any<string>(), Arg.Any<object>());
        Assert.True(result.IsSuccess);
    }
}