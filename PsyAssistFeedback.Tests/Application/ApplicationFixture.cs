using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using PsyAssistFeedback.Application.Interfaces.Repository;
using PsyAssistFeedback.Application.Interfaces.Service;
using PsyAssistFeedback.Application.Mapping;
using PsyAssistFeedback.Application.Services;
using PsyAssistPlatform.Domain;

namespace PsyAssistFeedback.Tests.Application;

public class ApplicationFixture : IDisposable
{
    public Mock<IRepository<Feedback>> FeedbackRepositoryMock { get; init; }

    public IFeedbackService FeedbackService { get; init; }

    public ApplicationFixture()
    {
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        FeedbackRepositoryMock = new Mock<IRepository<Feedback>>();
        FeedbackService = new FeedbackService(FeedbackRepositoryMock.Object, mapper, memoryCache);
    }

    public void Dispose() { }
}