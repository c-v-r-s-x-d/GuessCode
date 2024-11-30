using GuessCode.DAL.Models;
using GuessCode.DAL.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Email).IsRequired();

        builder
            .HasOne(x => x.UserProfile)
            .WithOne(x => x.User)
            .HasForeignKey<UserProfile>(x => x.UserId);

        builder
            .HasOne(x => x.GitHubProfile)
            .WithOne(x => x.User)
            .HasForeignKey<GitHubProfile>(x => x.UserId);

        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId);

        builder
            .HasMany(x => x.AuthoredKatas)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);

        builder
            .HasMany(x => x.ResolvedKatas)
            .WithMany()
            .UsingEntity(x => x.ToTable("Kata_User_Resolved"));
    }
}