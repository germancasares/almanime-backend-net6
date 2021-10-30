using API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Repositories.Configurations;

public class FansubConfiguration : BaseModelConfiguration<Fansub>
{
    public override void Configure(EntityTypeBuilder<Fansub> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(i => i.FullName)
            .IsUnique();

        builder
            .HasIndex(i => i.Acronym)
            .IsUnique();
    }
}
