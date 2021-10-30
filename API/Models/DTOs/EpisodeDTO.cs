namespace API.Models.DTOs;

public record EpisodeDTO
{
    public int Number { get; set; }
    public string? Name { get; set; }
    public DateTime? Aired { get; set; }
    public int? Duration { get; set; }

    public string? AnimeSlug { get; set; }
}
