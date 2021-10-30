namespace API.Models.Views;

public record EpisodeWithSubtitleView
{
    public int Number { get; set; }
    public string? Name { get; set; }
    public DateTime? Aired { get; set; }
    public int? Duration { get; set; }

    public SubtitleView? Subtitle { get; set; }
}
