using API.Models;

namespace API.Repositories.Interfaces;

public interface IFansubRepository : IBaseRepository<Fansub>
{
    void DeleteMembers(Guid fansubID);
    Fansub? GetByAcronym(string acronym);
    Fansub? GetByFullName(string fullname);
}
