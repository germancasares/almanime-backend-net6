using API.Models.Enums;

namespace API.Models.DTOs;

public record SubtitleDTO
{
    public string? FansubAcronym { get; set; }
    public string? AnimeSlug { get; set; }
    public int EpisodeNumber { get; set; }

    public IFormFile? Subtitle { get; set; }
    public ESubtitleStatus Status { get; set; }
}
