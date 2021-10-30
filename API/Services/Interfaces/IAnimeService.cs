using API.Models;
using API.Models.DTOs;
using API.Models.Enums;

namespace API.Services.Interfaces;

public interface IAnimeService
{
    Anime? GetByID(Guid guid);
    Anime? GetByKitsuID(int kitsuID);
    Anime? GetBySlug(string slug);

    Anime Create(AnimeDTO animeDTO);
    void Update(AnimeDTO animeDTO);
    void BulkMerge(IEnumerable<AnimeDTO> animes);
    IQueryable<Anime> GetSeason(int year, ESeason season);
    int GetAnimesInSeason(int year, ESeason season);
}
