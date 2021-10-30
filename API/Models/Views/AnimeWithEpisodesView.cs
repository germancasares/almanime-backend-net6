namespace API.Models.Views;

public record AnimeWithEpisodesView : AnimeView
{
    public ICollection<EpisodeView> Episodes { get; set; } = default!;
}
