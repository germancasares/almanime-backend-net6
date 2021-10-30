namespace API.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    void Save();

    IAnimeRepository Animes { get; }
    IBookmarkRepository Bookmarks { get; }
    IEpisodeRepository Episodes { get; }
    IFansubRepository Fansubs { get; }
    IMembershipRepository Memberships { get; }
    IStorageRepository Storage { get; }
    ISubtitleRepository Subtitles { get; }
    ISubtitlePartialRepository SubtitlePartials { get; }
    IUserRepository Users { get; }
}
