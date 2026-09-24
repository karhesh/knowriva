using Knowriva.Domain.Entities.LearningGoals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Knowriva.Infrastructure.Data.Configurations;

public sealed class LearningGoalConfiguration : IEntityTypeConfiguration<LearningGoal>
{
    public void Configure(EntityTypeBuilder<LearningGoal> builder)
    {
        builder.ToTable("LearningGoals");

        builder.HasKey(goal => goal.Id);
        builder.Property(goal => goal.Id).ValueGeneratedNever();

        builder.Property(goal => goal.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(goal => goal.Description)
            .HasMaxLength(1000);
    }
}
