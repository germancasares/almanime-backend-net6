using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class FansubRepository : BaseRepository<Fansub>, IFansubRepository
{
    public FansubRepository(AlmanimeContext context) : base(context) { }

    public void DeleteMembers(Guid fansubID)
    {
        var fansub = GetByID(fansubID);

        if (fansub != null) fansub.Memberships.Clear();
    }

    public Fansub? GetByFullName(string fullname) => GetAll().SingleOrDefault(f => f.FullName == fullname);
    public Fansub? GetByAcronym(string acronym) => GetAll().SingleOrDefault(f => f.Acronym == acronym);
}
