using GuessCode.DAL.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class MentorRequestConfiguration : IEntityTypeConfiguration<MentorRequest>
{
    public void Configure(EntityTypeBuilder<MentorRequest> builder)
    {
        builder.ToTable("MentorRequest");

        builder.HasKey(x => new { x.UserId, x.MentorId });
    }
}