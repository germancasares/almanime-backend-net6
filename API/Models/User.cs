using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class User : IdentityUser<Guid>
{
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = default!;
    public virtual ICollection<Bookmark> Bookmarks { get; set; } = default!;
    public virtual ICollection<SubtitlePartial> SubtitlePartials { get; set; } = default!;
}
