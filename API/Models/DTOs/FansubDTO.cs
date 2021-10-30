using API.Models.Enums;

namespace API.Models.DTOs;

public record FansubDTO
{
    public string? Acronym { get; set; }
    public string? FullName { get; set; }
    public string? Webpage { get; set; }
    public EFansubMainLanguage MainLanguage { get; set; }
    public EFansubMembershipOption MembershipOption { get; set; }
}
