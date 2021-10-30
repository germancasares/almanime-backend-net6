using API.Models;
using API.Models.DTOs;

namespace API.Utils.Mappers;

public static class EpisodeMapper
{
    public static Episode MapToModel(this EpisodeDTO episodeDTO, Guid animeID)
    {
        return new Episode(
            number: episodeDTO.Number, 
            name: episodeDTO.Name ?? throw new ArgumentNullException(nameof(episodeDTO), "The value of 'episodeDTO.Name' should not be null"),
            aired: episodeDTO.Aired, 
            duration: episodeDTO.Duration, 
            animeID: animeID
        );
    }
}
