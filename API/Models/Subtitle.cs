using API.Models.Enums;

namespace API.Models;

public class Subtitle : Base
{
    public Subtitle(ESubtitleStatus status, Guid episodeID, Guid fansubID)
    {
        Status = status;
        EpisodeID = episodeID;
        FansubID = fansubID;
    }

    public Subtitle(ESubtitleStatus status, Guid episodeID, Guid fansubID, ESubtitleFormat format)
    {
        Status = status;
        EpisodeID = episodeID;
        FansubID = fansubID;
        Format = format;
    }

    public ESubtitleStatus Status { get; set; }

    public Guid EpisodeID { get; set; }
    public virtual Episode Episode { get; set; } = default!;

    public Guid FansubID { get; set; }
    public virtual Fansub Fansub { get; set; } = default!;

    public string? Url { get; set; }
    public ESubtitleFormat? Format { get; set; }
    public virtual ICollection<SubtitlePartial> SubtitlePartials { get; set; } = default!;
}
