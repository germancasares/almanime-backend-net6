using API.Models;

namespace API.Repositories.Interfaces;

public interface IBookmarkRepository : IBaseRepository<Bookmark>
{
    Bookmark? Get(string animeSlug, Guid userID);
    IQueryable<Bookmark> GetByUserID(Guid userID);
}
