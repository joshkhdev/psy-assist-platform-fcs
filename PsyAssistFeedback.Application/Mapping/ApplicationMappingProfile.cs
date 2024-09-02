using AutoMapper;
using PsyAssistFeedback.Application.Dto.Feedback;
using PsyAssistFeedback.Application.Interfaces.Dto.Feedback;
using PsyAssistPlatform.Domain;
using PsyAssistPlatform.Messages;

namespace PsyAssistFeedback.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateFeedbackMap();
    }

    private void CreateFeedbackMap()
    {
        CreateMap<Feedback, FeedbackDto>();
        CreateMap<ICreateFeedback, Feedback>();
        CreateMap<FeedbackMessage, CreateFeedbackDto>();
    }
}