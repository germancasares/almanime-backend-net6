using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class BookmarkRepository : BaseRepository<Bookmark>, IBookmarkRepository
{
    public BookmarkRepository(AlmanimeContext context) : base(context) { }

    public IQueryable<Bookmark> GetByUserID(Guid userID) => GetAll().Where(b => b.UserID == userID);

    public Bookmark? Get(string animeSlug, Guid userID) => GetByUserID(userID).SingleOrDefault(b => b.Anime.Slug == animeSlug);
}
