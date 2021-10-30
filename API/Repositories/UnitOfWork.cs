using API.Repositories.Interfaces;

namespace API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AlmanimeContext _context;

    public UnitOfWork(
        AlmanimeContext context,

        IAnimeRepository animes,
        IBookmarkRepository bookmarks,
        IEpisodeRepository episodes,
        IFansubRepository fansubs,
        IMembershipRepository memberships,
        IStorageRepository storage,
        ISubtitleRepository subtitles,
        ISubtitlePartialRepository subtitlesPartials,
        IUserRepository users
    )
    {
        _context = context;

        Animes = animes;
        Bookmarks = bookmarks;
        Episodes = episodes;
        Fansubs = fansubs;
        Memberships = memberships;
        Storage = storage;
        Subtitles = subtitles;
        SubtitlePartials = subtitlesPartials;
        Users = users;
    }

    public IAnimeRepository Animes { get; }
    public IBookmarkRepository Bookmarks { get; }
    public IEpisodeRepository Episodes { get; }
    public IFansubRepository Fansubs { get; }
    public IMembershipRepository Memberships { get; }
    public IStorageRepository Storage { get; }
    public ISubtitleRepository Subtitles { get; }
    public ISubtitlePartialRepository SubtitlePartials { get; }
    public IUserRepository Users { get; }


    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            _context.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Save() => _context.SaveChanges();
}
