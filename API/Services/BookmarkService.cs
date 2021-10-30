using API.Models;
using API.Models.Enums;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils;

namespace API.Services;

public class BookmarkService : IBookmarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookmarkService> _logger;

    public BookmarkService(
        IUnitOfWork unitOfWork,
        ILogger<BookmarkService> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public IEnumerable<string> GetByUserID(Guid userID) => _unitOfWork.Bookmarks.GetByUserID(userID).Select(b => b.Anime.Slug);

    public Bookmark Create(string slug, Guid id)
    {
        var anime = _unitOfWork.Animes.GetBySlug(slug);
        if (anime == null)
        {
            _logger.Emit(ELoggingEvent.AnimeSlugDoesntExist, new { AnimeSlug = slug });
            throw new ArgumentException(nameof(slug));
        }

        var user = _unitOfWork.Users.GetByID(id);
        if (user == null)
        {
            _logger.Emit(ELoggingEvent.UserDoesntExist, new { UserID = id });
            throw new ArgumentException(nameof(id));
        }

        var bookmark = _unitOfWork.Bookmarks.Get(slug, user.Id);
        if (bookmark != null)
        {
            _logger.Emit(ELoggingEvent.BookmarkAlreadyExists, new { AnimeSlug = slug, UserID = id });
            throw new ArgumentException($"{nameof(slug)} & {nameof(id)}");
        }

        bookmark = _unitOfWork.Bookmarks.Create(new Bookmark
        {
            AnimeID = anime.ID,
            UserID = user.Id,
        });

        _unitOfWork.Save();

        return bookmark;
    }

    public void Delete(string animeSlug, Guid identityID)
    {
        //var user = _unitOfWork.Users.GetByIdentityID(identityID);
        //if (user == null)
        //{
        //    throw new ArgumentException(nameof(identityID));
        //}

        //var bookmark = _unitOfWork.Bookmarks.Get(animeSlug, user.ID);
        //if (bookmark == null)
        //{
        //    _logger.Emit(ELoggingEvent.BookmarkDoesntExist, new { AnimeSlug = animeSlug, UserIdentityID = identityID });
        //    return;
        //}

        //_unitOfWork.Bookmarks.Delete(bookmark);
        //_unitOfWork.Save();
    }
}
