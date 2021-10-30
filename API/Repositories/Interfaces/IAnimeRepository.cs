using API.Models;
using API.Models.Enums;

namespace API.Repositories.Interfaces;

public interface IAnimeRepository : IBaseRepository<Anime>
{
    Anime? GetByKitsuID(int kitsuID);
    Anime? GetBySlug(string slug);
    int GetAnimesInSeason(int year, ESeason season);
    IQueryable<Anime> GetSeason(int year, ESeason season);
    IQueryable<Anime> GetByFansub(string acronym);
    IQueryable<Anime> GetCompletedByFansub(string acronym);
}
