namespace API.Models.Views;

public record FansubEpisodeView
{
    public string? AnimeSlug { get; set; }
    public string? AnimeName { get; set; }
    public string? AnimeCoverImage { get; set; }
    public int Number { get; set; }
    public string? Name { get; set; }
    public DateTime FinishedDate { get; set; }
}
