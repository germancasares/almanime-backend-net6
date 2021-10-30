using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetByID(Guid id);
        IQueryable<User> GetByFansub(string acronym);
        User? GetByName(string name);

        User Create(User entity);
        void Update(User entity);
    }
}
