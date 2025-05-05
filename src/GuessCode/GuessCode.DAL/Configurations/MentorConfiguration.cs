using GuessCode.DAL.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class MentorConfiguration : IEntityTypeConfiguration<Mentor>
{
    public void Configure(EntityTypeBuilder<Mentor> builder)
    {
        builder.ToTable("Mentor",
            t => t.HasCheckConstraint("CK_Review_Rating_Range", "\"Rating\" >= 0.0 AND \"Rating\" <= 5.0"));

        builder.Property(m => m.Rating)
            .IsRequired()
            .HasDefaultValue(0.0);

        builder
            .HasMany(x => x.Mentees)
            .WithOne()
            .HasForeignKey(x => x.MentorId);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Mentor)
            .HasForeignKey<Mentor>(x => x.UserId);
    }
}