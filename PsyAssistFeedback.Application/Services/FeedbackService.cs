using AutoMapper;
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

    public FeedbackService(
        IRepository<Feedback> feedbackRepository,
        IMapper applicationMapper)
    {
        _feedbackRepository = feedbackRepository;
        _applicationMapper = applicationMapper;
    }

    public async Task<IEnumerable<IFeedback>> GetFeedbacksAsync(CancellationToken cancellationToken)
    {
        var feedbacks = await _feedbackRepository.GetAllAsync(cancellationToken);
        return _applicationMapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
    }

    public async Task<IFeedback?> GetFeedbackByIdAsync(int id, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(id, cancellationToken);

        if (feedback is null)
            throw new NotFoundException($"Feedback with Id [{id}] not found");

        return _applicationMapper.Map<FeedbackDto>(feedback);
    }

    public async Task<IFeedback> CreateFeedbackAsync(ICreateFeedback feedbackData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(feedbackData.Telegram))
            throw new IncorrectDataException("Telegram cannot be empty");

        if (string.IsNullOrWhiteSpace(feedbackData.FeedbackText))
            throw new IncorrectDataException(
                "Feedback text cannot be null or empty");

        var createdFeedback = _applicationMapper.Map<Feedback>(feedbackData);
        createdFeedback.FeedbackDate = DateTime.Now.ToUniversalTime();

        return _applicationMapper.Map<FeedbackDto>(
            await _feedbackRepository.AddAsync(createdFeedback, cancellationToken));
    }
}