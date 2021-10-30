using API.Models.Enums;

namespace API.Models;

public class Fansub : Base
{
    public Fansub(string acronym, string fullName, EFansubMainLanguage mainLanguage, EFansubMembershipOption membershipOption)
    {
        Acronym = acronym ?? throw new ArgumentNullException(nameof(acronym));
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        MainLanguage = mainLanguage;
        MembershipOption = membershipOption;
    }

    public string Acronym { get; set; }
    public string FullName { get; set; }
    public string? LogoUrl { get; set; }
    public string? Webpage { get; set; }
    public EFansubMainLanguage MainLanguage { get; set; }
    public EFansubMembershipOption MembershipOption { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = default!;
    public virtual ICollection<Subtitle> Subtitles { get; set; } = default!;
}
