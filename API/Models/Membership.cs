using API.Models.Enums;

namespace API.Models;

public class Membership : Base
{
    public Guid FansubID { get; set; }
    public virtual Fansub Fansub { get; set; } = default!;

    public Guid UserID { get; set; }
    public virtual User User { get; set; } = default!;

    public EFansubRole Role { get; set; }
}
