using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.WebApi.Models.Feedback;

namespace PsyAssistFeedback.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    private readonly IMapper _mapper;

    public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
    {
        _feedbackService = feedbackService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список всех отзывов
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync(CancellationToken cancellationToken)
    {
        var feedbacks = await _feedbackService.GetFeedbacksAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FeedbackResponse>>(feedbacks);
    }

    /// <summary>
    /// Получить отзыв по Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeedbackResponse>> GetFeedbackByIdAsync(int id, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackService.GetFeedbackByIdAsync(id, cancellationToken);
        return _mapper.Map<FeedbackResponse>(feedback);
    }

    /// <summary>
    /// Создать новый отзыв
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFeedbackAsync(CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        await _feedbackService.CreateFeedbackAsync(request, cancellationToken);
        return Ok();
    }
}
