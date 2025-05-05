using GuessCode.DAL.Models.KataAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessCode.DAL.Configurations;

public class KataTestFileConfiguration : IEntityTypeConfiguration<KataTestFile>
{
    public void Configure(EntityTypeBuilder<KataTestFile> builder)
    {
        builder.ToTable("KataTestFile");
        builder.HasKey(x => x.KataId);
    }
}