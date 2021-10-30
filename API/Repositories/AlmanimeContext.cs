using API.Models;
using API.Repositories.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Repositories;

public class AlmanimeContext : IdentityDbContext<User, Role, Guid>
{
    public AlmanimeContext(DbContextOptions<AlmanimeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AnimeConfiguration());
        modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
        modelBuilder.ApplyConfiguration(new EpisodeConfiguration());
        modelBuilder.ApplyConfiguration(new FansubConfiguration());
        modelBuilder.ApplyConfiguration(new MembershipConfiguration());
        modelBuilder.ApplyConfiguration(new SubtitleConfiguration());
        modelBuilder.ApplyConfiguration(new SubtitlePartialConfiguration());
        // modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    public DbSet<Anime> Animes => Set<Anime>();
    public DbSet<Episode> Episodes => Set<Episode>();

    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<Fansub> Fansubs => Set<Fansub>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Subtitle> Subtitles => Set<Subtitle>();
    public DbSet<SubtitlePartial> SubtitlePartials => Set<SubtitlePartial>();
}
