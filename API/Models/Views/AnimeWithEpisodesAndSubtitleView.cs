using API.Models.Enums;

namespace API.Models.Views;

public record AnimeWithEpisodesAndSubtitleView
{
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public ESeason Season { get; set; }
    public DateTime StartDate { get; set; }

    public int EpisodesCount { get; set; }
    public ICollection<EpisodeWithSubtitleView> Episodes { get; set; } = default!;
}
