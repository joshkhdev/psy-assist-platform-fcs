using FluentAssertions;
using Moq;
using PsyAssistFeedback.Application.Exceptions;
using PsyAssistFeedback.Application.Interfaces.Repository;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.Tests.Application;
using PsyAssistFeedback.WebApi.Models.Feedback;
using PsyAssistPlatform.Domain;
using System.Linq.Expressions;

namespace PsyAssistFeedback.Tests.Application.Services;

public class FeedbackServiceTests : IClassFixture<ApplicationFixture>
{
    private readonly Mock<IRepository<Feedback>> _feedbackRepositoryMock;
    private readonly IFeedbackService _feedbackService;

    public FeedbackServiceTests(ApplicationFixture applicationFixture)
    {
        _feedbackRepositoryMock = applicationFixture.FeedbackRepositoryMock;
        _feedbackService = applicationFixture.FeedbackService;
    }

    #region GetFeedbackByIdAsync

    [Fact]
    public async Task GetFeedbackByIdAsync_ValidData_Success()
    {
        // Arrange
        const int feedbackId = 2;
        _feedbackRepositoryMock
            .Setup(repository => repository.GetByIdAsync(feedbackId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetGoodFeedback());

        // Act
        var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId, default);

        // Assert
        feedback!.Telegram.Should().Be(GetGoodFeedback().Telegram);
        feedback!.FeedbackText.Should().Be(GetGoodFeedback().FeedbackText);
    }

    [Fact]
    public async Task GetFeedbackByIdAsync_FeedbackIsNotFound_ThrowNotFoundException()
    {
        // Arrange
        const int feedbackId = 1;
        _feedbackRepositoryMock
            .Setup(repository => repository.GetByIdAsync(feedbackId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Feedback)null!);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _feedbackService.GetFeedbackByIdAsync(feedbackId, default));

        // Assert
        exception.Should().NotBeNull();
    }

    #endregion

    #region CreateFeedbackAsync

    [Fact]
    public async Task CreateFeedbackAsync_ValidData_Success()
    {
        // Arrange
        var createFeedbackRequest = new CreateFeedbackRequest()
        {
            Telegram = "test",
            FeedbackText = "Good"
        };

        _feedbackRepositoryMock.Setup(repository =>
                repository.GetAsync(It.IsAny<Expression<Func<Feedback, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _feedbackRepositoryMock.Setup(repository =>
                repository.AddAsync(
                    It.Is<Feedback>(feedback => feedback.Telegram == createFeedbackRequest.Telegram
                        && feedback.FeedbackText == createFeedbackRequest.FeedbackText),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetGoodFeedback());

        // Act
        var feedback = await _feedbackService.CreateFeedbackAsync(createFeedbackRequest, default);

        // Assert
        feedback.Telegram.Should().Be(GetGoodFeedback().Telegram);
        feedback.FeedbackText.Should().Be(GetGoodFeedback().FeedbackText);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", " ")]
    [InlineData(" ", "")]
    [InlineData(" ", " ")]
    public async Task CreateFeedbackAsync_FeedbackIsEmptyOrWhiteSpace_ThrowIncorrectDataException(string telegram, string feedbackText)
    {
        // Arrange
        var createFeedbackRequest = new CreateFeedbackRequest()
        {
            Telegram = telegram,
            FeedbackText = feedbackText
        };

        // Act
        var exception = await Assert.ThrowsAsync<IncorrectDataException>(() =>
            _feedbackService.CreateFeedbackAsync(createFeedbackRequest, default));

        // Assert
        exception.Should().NotBeNull();
    }

    #endregion

    private static Feedback GetGoodFeedback() => new()
    {
        Id = 2,
        Telegram = "test",
        FeedbackText = "Good",
        FeedbackDate = DateTime.Now
    };
}