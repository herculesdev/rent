using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rent.Backoffice.Core.Data;
using Rent.Backoffice.Core.Entities;
using Rent.Backoffice.Core.Features.Motorbike.GetPaged;
using Rent.Shared.Library.Pagination;

namespace Rent.Backoffice.Tests.Core.Features.GetPaged;

public class GetMotorbikesPagedQueryHandlerTests
{
    private readonly IMotorbikeRepository _motorbikeRepository;
    private readonly ILogger<GetMotorbikesPagedQueryHandler> _logger;
    private readonly GetMotorbikesPagedQueryHandler _getMotorbikesPagedQueryHandler;
    private readonly GetMotorbikesPagedQuery _getMotorbikesPagedQuerySample = new("", 1, 10);

    public GetMotorbikesPagedQueryHandlerTests()
    {
        _motorbikeRepository = Substitute.For<IMotorbikeRepository>();
        _logger = Substitute.For<ILogger<GetMotorbikesPagedQueryHandler>>();
        
        _getMotorbikesPagedQueryHandler = new GetMotorbikesPagedQueryHandler(_motorbikeRepository, _logger);
    }
    
    [Fact]
    public async Task Handle_ShouldSucceed()
    {
        var motorbikes = new List<Motorbike>()
        {
            new(Guid.NewGuid(), 2024, "Honda Titan 160", "HON4J09"),
            new(Guid.NewGuid(), 2023, "Yamaha Factor 150", "YAM094J"),
            new(Guid.NewGuid(), 2023, "Dafra Speed 150", "DAF0B25"),
        };
        var pagedMotorbikes = new Paged<Motorbike>(motorbikes, 1, 10, 3);
        var expectedMotorbikesCount = motorbikes.Count;
        _motorbikeRepository.GetPaged(_getMotorbikesPagedQuerySample).Returns(pagedMotorbikes);
        
        var result = await _getMotorbikesPagedQueryHandler.Handle(_getMotorbikesPagedQuerySample);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedMotorbikesCount, result.Items.Count());
    }
}