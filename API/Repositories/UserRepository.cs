using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AlmanimeContext _context;

        public UserRepository(AlmanimeContext context) => _context = context;

        public IQueryable<User> GetAll() => _context.Set<User>().AsQueryable();

        public User? GetByID(Guid id) => GetAll().SingleOrDefault(u => u.Id == id);

        public User? GetByName(string name) => GetAll().SingleOrDefault(u => u.UserName == name);

        public IQueryable<User> GetByFansub(string acronym) => GetAll().Where(u => u.Memberships.Any(m => m.Fansub.Acronym == acronym));

        public User Create(User entity) => _context.Set<User>().Add(entity).Entity;

        public void Update(User entity)
        {
            entity.ModificationDate = DateTime.UtcNow;

            _context.Set<User>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
