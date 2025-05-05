using GuessCode.DAL.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class MentorConfiguration : IEntityTypeConfiguration<Mentor>
{
    public void Configure(EntityTypeBuilder<Mentor> builder)
    {
        builder.ToTable("Mentor",
            t => t.HasCheckConstraint("CK_Review_Rating_Range", "rating >= 0.0 AND rating <= 5.0"));

        builder.Property(m => m.Rating)
            .IsRequired()
            .HasDefaultValue(0.0);
    }
}