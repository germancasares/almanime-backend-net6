using API.Models;

namespace API.Repositories.Interfaces;

public interface IMembershipRepository
{
    Membership Create(Membership member);
    bool IsFounder(Guid fansubID, Guid userTrigger);
}
