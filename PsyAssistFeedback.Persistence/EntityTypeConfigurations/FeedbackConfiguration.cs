using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PsyAssistPlatform.Domain;

namespace PsyAssistFeedback.Persistence.EntityTypeConfigurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(feedback => feedback.Id);
        builder.HasIndex(feedback => feedback.Id);
        builder.Property(feedback => feedback.Telegram).HasMaxLength(50);
        builder.Property(feedback => feedback.FeedbackText).HasMaxLength(1000);
    }
}