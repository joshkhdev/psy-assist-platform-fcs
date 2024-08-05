using AutoMapper;
using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;
using PsyAssistFeedback.WebApi.Models.Feedback;

namespace PsyAssistFeedback.WebApi.Mapping;

public class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        CreateFeedbackMap();
    }

    private void CreateFeedbackMap()
    {
        CreateMap<IFeedback, FeedbackResponse>();
    }
}