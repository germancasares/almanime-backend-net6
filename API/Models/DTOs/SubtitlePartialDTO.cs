namespace API.Models.DTOs;

public record SubtitlePartialDTO
{
    public string? FansubAcronym { get; set; }
    public string? AnimeSlug { get; set; }
    public int EpisodeNumber { get; set; }
    public IFormFile? Partial { get; set; }
}
