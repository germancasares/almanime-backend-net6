namespace API.Models;

public class SubtitlePartial : Base
{
    public SubtitlePartial(Guid subtitleID, Guid userID)
    {
        SubtitleID = subtitleID;
        UserID = userID;
    }

    public string? Url { get; set; }

    public Guid SubtitleID { get; set; }
    public virtual Subtitle Subtitle { get; set; } = default!;

    public Guid UserID { get; set; }
    public virtual User User { get; set; } = default!;
}
