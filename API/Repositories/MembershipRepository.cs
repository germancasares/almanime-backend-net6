using API.Models;
using API.Models.Enums;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
{
    public MembershipRepository(AlmanimeContext context) : base(context) { }

    public bool IsFounder(Guid fansubID, Guid userTrigger) => GetAll().Any(m =>
        m.FansubID == fansubID
        && m.UserID == userTrigger
        && m.Role == EFansubRole.Founder
    );
}
