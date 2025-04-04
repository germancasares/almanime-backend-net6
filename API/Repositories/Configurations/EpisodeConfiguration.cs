﻿using API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Repositories.Configurations;

public class EpisodeConfiguration : BaseModelConfiguration<Episode>
{
    public override void Configure(EntityTypeBuilder<Episode> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(c => c.Anime)
            .WithMany(c => c.Episodes)
            .HasForeignKey(c => c.AnimeID);
    }
}
