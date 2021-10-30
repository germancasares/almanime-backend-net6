using API.Models.Enums;

namespace API.Models.Views;

public record FansubUserView
{
    public string? AvatarUrl { get; set; }
    public string? Name { get; set; }
    public EFansubRole Role { get; set; }
}
