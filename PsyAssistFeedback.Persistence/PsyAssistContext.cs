using Microsoft.EntityFrameworkCore;
using PsyAssistFeedback.Persistence.EntityTypeConfigurations;
using PsyAssistPlatform.Domain;

namespace PsyAssistFeedback.Persistence;

public class PsyAssistContext : DbContext
{
    public PsyAssistContext(DbContextOptions<PsyAssistContext> options) : base(options)
    {
    }

    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new FeedbackConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseCamelCaseNamingConvention();
}