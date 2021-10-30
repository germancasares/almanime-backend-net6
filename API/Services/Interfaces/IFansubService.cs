using API.Models;
using API.Models.DTOs;
using API.Models.Views;

namespace API.Services.Interfaces;

public interface IFansubService
{
    Fansub Create(FansubDTO fansubDTO, Guid identityID);
    void Delete(Guid fansubID, Guid identityID);
    IEnumerable<FansubAnimeView> GetCompletedAnimes(string acronym);
    Fansub? GetByAcronym(string acronym);
    Fansub? GetByID(Guid ID);
    bool ExistsFullName(string fullname);
    bool ExistsAcronym(string acronym);
    IEnumerable<FansubEpisodeView> GetCompletedEpisodes(string acronym);
    IEnumerable<FansubUserView> GetMembers(string acronym);
}
