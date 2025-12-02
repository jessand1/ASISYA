using AutoMapper;
using Moq;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Optimization.DTOs;
using ProviderOptimizer.Application.Optimization.Handlers;
using ProviderOptimizer.Application.Optimization.Queries;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

public class GetOptimizationResultsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<OptimizationResult>> _repoMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly GetOptimizationResultsQueryHandler _handler;

    public GetOptimizationResultsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repoMock = new Mock<IGenericRepository<OptimizationResult>>();
        _mapperMock = new Mock<IMapper>();

        // Setup del repositorio dentro del UoW
        _unitOfWorkMock
            .Setup(u => u.Repository<OptimizationResult>())
            .Returns(_repoMock.Object);

        _handler = new GetOptimizationResultsQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedResults_WhenDataExists()
    {
        // Arrange
        var requestId = Guid.NewGuid();

        var query = new GetOptimizationResultsQuery(requestId);

        var domainResults = new List<OptimizationResult>
        {
            new OptimizationResult { ResultId = Guid.NewGuid(), RequestId = requestId, ProviderId = Guid.NewGuid(), Score = 90 },
            new OptimizationResult { ResultId = Guid.NewGuid(), RequestId = requestId, ProviderId = Guid.NewGuid(), Score = 80 }
        };

        var dtoResults = new List<OptimizationResultDto>
        {
            new OptimizationResultDto { ProviderId = domainResults[0].ProviderId, Score = 90 },
            new OptimizationResultDto { ProviderId = domainResults[1].ProviderId, Score = 80 }
        };

        _repoMock
            .Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OptimizationResult, bool>>>()))
            .ReturnsAsync(domainResults);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<OptimizationResultDto>>(domainResults))
            .Returns(dtoResults);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dtoResults, result);

        _unitOfWorkMock.Verify(u => u.Repository<OptimizationResult>(), Times.Once);
        _repoMock.Verify(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OptimizationResult, bool>>>()), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<OptimizationResultDto>>(domainResults), Times.Once);
    }
}
