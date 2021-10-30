using API.Models;
using API.Models.DTOs;

namespace API.Utils.Mappers;

public static class FansubMapper
{
    public static Fansub MapToModel(this FansubDTO fansubDTO)
    {
        return new Fansub(
            acronym: fansubDTO.Acronym ?? throw new ArgumentNullException(nameof(fansubDTO), "The value of 'fansubDTO.Acronym' should not be null"), 
            fullName: fansubDTO.FullName ?? throw new ArgumentNullException(nameof(fansubDTO), "The value of 'fansubDTO.FullName' should not be null"), 
            mainLanguage: fansubDTO.MainLanguage, 
            membershipOption: fansubDTO.MembershipOption
        );
    }
}
