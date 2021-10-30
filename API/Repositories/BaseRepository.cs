using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : Base
{
    private readonly AlmanimeContext _context;

    public BaseRepository(AlmanimeContext context) => _context = context;

    public IQueryable<TModel> GetAll() => _context.Set<TModel>().AsQueryable();
    public TModel? GetByID(Guid id) => _context.Set<TModel>().SingleOrDefault(o => o.ID == id);

    public TModel Create(TModel entity) => _context.Set<TModel>().Add(entity).Entity;

    public void Update(TModel entity)
    {
        entity.ModificationDate = DateTime.UtcNow;

        _context.Set<TModel>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void BulkMerge(IEnumerable<TModel> entities) => _context.Set<TModel>().BulkMerge(entities);

    public void Delete(Guid id)
    {
        var entity = GetByID(id);

        if (entity is not null) Delete(entity);
    }

    public void Delete(TModel entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<TModel>().Attach(entity);
        }
        _context.Set<TModel>().Remove(entity);
    }
}
