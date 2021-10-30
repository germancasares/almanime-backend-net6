using API.Models.Enums;

namespace API.Models.Views;

public record SubtitleView
{
    public ESubtitleStatus Status { get; set; }
    public string? Format { get; set; }
    public string? Url { get; set; }
    public DateTime ModificationDate { get; set; }
}
