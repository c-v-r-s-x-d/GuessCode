using GuessCode.DAL.Models.KataAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class KataConfiguration : IEntityTypeConfiguration<Kata>
{
    public void Configure(EntityTypeBuilder<Kata> builder)
    {
        builder.ToTable("Kata");

        builder.Property(k => k.MemoryLimits)
            .HasColumnType("jsonb");
        
        builder.Property(k => k.TimeLimits)
            .HasColumnType("jsonb");
    }
}