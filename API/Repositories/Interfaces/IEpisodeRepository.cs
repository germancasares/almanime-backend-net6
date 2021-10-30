using API.Models;

namespace API.Repositories.Interfaces;

public interface IEpisodeRepository : IBaseRepository<Episode>
{
    Episode? GetByAnimeIDAndNumber(Guid guid, int number);
    Episode? GetByAnimeSlugAndNumber(string animeSlug, int number);
    IQueryable<Episode> GetByFansub(string acronym);
    IQueryable<Episode> GetByAnimeID(Guid guid);
    IQueryable<Episode> GetByAnimeSlug(string animeSlug);
}
