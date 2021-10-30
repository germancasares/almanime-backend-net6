using API.Models;
using API.Models.DTOs;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils.Mappers;
using AutoMapper;

namespace API.Services;

public class EpisodeService : IEpisodeService
{
    private readonly IUnitOfWork _unitOfWork;

    public Episode? GetByID(Guid guid) => _unitOfWork.Episodes.GetByID(guid);
    public Episode? GetByAnimeIDAndNumber(Guid guid, int number) => _unitOfWork.Episodes.GetByAnimeIDAndNumber(guid, number);
    public Episode? GetByAnimeSlugAndNumber(string animeSlug, int number) => _unitOfWork.Episodes.GetByAnimeSlugAndNumber(animeSlug, number);
    public IEnumerable<Episode> GetByAnimeID(Guid guid) => _unitOfWork.Episodes.GetByAnimeID(guid);
    public IEnumerable<Episode> GetByAnimeSlug(string animeSlug) => _unitOfWork.Episodes.GetByAnimeSlug(animeSlug);

    public EpisodeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Episode? Create(EpisodeDTO episodeDTO)
    {
        if (episodeDTO.AnimeSlug == null) return null;

        var anime = _unitOfWork.Animes.GetBySlug(episodeDTO.AnimeSlug);

        if (anime == null) return null;

        var episode = _unitOfWork.Episodes.Create(episodeDTO.MapToModel(anime.ID));

        _unitOfWork.Save();

        return episode;
    }

    public void Update(EpisodeDTO episodeDTO)
    {
        if (episodeDTO.AnimeSlug == null) return;

        var anime = _unitOfWork.Animes.GetBySlug(episodeDTO.AnimeSlug);

        if (anime == null) return;

        var episode = GetByAnimeIDAndNumber(anime.ID, episodeDTO.Number);

        if (episode == null) return;

        _unitOfWork.Episodes.Update(episodeDTO.MapToModel(anime.ID));
        _unitOfWork.Save();
    }
}
