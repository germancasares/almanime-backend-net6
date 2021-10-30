using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class SubtitleRepository : BaseRepository<Subtitle>, ISubtitleRepository
{
    public SubtitleRepository(AlmanimeContext context) : base(context) { }

    public IQueryable<Subtitle> GetByFansubAndAnime(string fansubAcronym, string animeSlug) => GetAll().Where(s => s.Fansub.Acronym == fansubAcronym && s.Episode.Anime.Slug == animeSlug);
}
