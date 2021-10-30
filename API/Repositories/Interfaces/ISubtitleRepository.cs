using API.Models;

namespace API.Repositories.Interfaces;

public interface ISubtitleRepository : IBaseRepository<Subtitle>
{
    IQueryable<Subtitle> GetByFansubAndAnime(string fansubAcronym, string animeSlug);
}
