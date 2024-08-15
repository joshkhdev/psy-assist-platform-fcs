using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Exceptions;
using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;
using PsyAssistFeedback.Application.Interfaces.Repository;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistPlatform.Domain;

namespace PsyAssistFeedback.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IRepository<Feedback> _feedbackRepository;
    private readonly IMapper _applicationMapper;
    private readonly IMemoryCache _memoryCache;

    private const string FeedbackNotFoundMessage = "Feedback with Id [{0}] not found";
    private const string EmptyTelegramMessage = "Telegram cannot be empty";
    private const string EmptyTextMessage = "Feedback text cannot be null or empty";
    private const string FeedbackCacheName = "Feedback_{0}";

    public FeedbackService(
        IRepository<Feedback> feedbackRepository,
        IMapper applicationMapper, 
        IMemoryCache memoryCache)
    {
        _feedbackRepository = feedbackRepository;
        _applicationMapper = applicationMapper;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<IFeedback>?> GetFeedbacksAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(FeedbackCacheName, "All");
        var feedbacks = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(24));

            var allFeedbacks = await _feedbackRepository.GetAllAsync(cancellationToken);
            return _applicationMapper.Map<IEnumerable<FeedbackDto>>(allFeedbacks);
        });

        return feedbacks;
    }

    public async Task<IFeedback?> GetFeedbackByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(FeedbackCacheName, id);

        var feedback = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(24));

            var feedbackById = await _feedbackRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(string.Format(FeedbackNotFoundMessage, id));
            return _applicationMapper.Map<FeedbackDto>(feedbackById);
        });

        return feedback;
    }

    public async Task<IFeedback> CreateFeedbackAsync(ICreateFeedback feedbackData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(feedbackData.Telegram))
            throw new IncorrectDataException(EmptyTelegramMessage);

        if (string.IsNullOrWhiteSpace(feedbackData.FeedbackText))
            throw new IncorrectDataException(EmptyTextMessage);

        _memoryCache.Remove(string.Format(FeedbackCacheName, "All"));

        var createdFeedback = _applicationMapper.Map<Feedback>(feedbackData);
        createdFeedback.FeedbackDate = DateTime.Now.ToUniversalTime();

        return _applicationMapper.Map<FeedbackDto>(
            await _feedbackRepository.AddAsync(createdFeedback, cancellationToken));
    }
}