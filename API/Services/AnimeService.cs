using API.Models;
using API.Models.DTOs;
using API.Models.Enums;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils.Mappers;

namespace API.Services;

public class AnimeService : IAnimeService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnimeService(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }

    public Anime? GetByID(Guid guid) => _unitOfWork.Animes.GetByID(guid);
    public Anime? GetByKitsuID(int kitsuID) => _unitOfWork.Animes.GetByKitsuID(kitsuID);
    public Anime? GetBySlug(string slug) => _unitOfWork.Animes.GetBySlug(slug);
    public int GetAnimesInSeason(int year, ESeason season) => _unitOfWork.Animes.GetAnimesInSeason(year, season);
    public IQueryable<Anime> GetSeason(int year, ESeason season) => _unitOfWork.Animes.GetSeason(year, season);

    public Anime Create(AnimeDTO animeDTO)
    {
        var anime = _unitOfWork.Animes.Create(animeDTO.MapToModel());

        _unitOfWork.Save();

        return anime;
    }

    public void Update(AnimeDTO animeDTO)
    {
        var anime = GetByKitsuID(animeDTO.KitsuID);

        if (anime == null) return;

        _unitOfWork.Animes.Update(anime.UpdateFromDTO(animeDTO));
        _unitOfWork.Save();
    }

    public void BulkMerge(IEnumerable<AnimeDTO> animes) => _unitOfWork.Animes.BulkMerge(animes.Select(a => a.MapToModel()));
}
