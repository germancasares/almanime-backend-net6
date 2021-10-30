using API.Models;

namespace API.Services.Interfaces;

public interface IBookmarkService
{
    Bookmark Create(string slug, Guid identityID);
    void Delete(string animeSlug, Guid identityID);
    IEnumerable<string> GetByUserID(Guid userID);
}
