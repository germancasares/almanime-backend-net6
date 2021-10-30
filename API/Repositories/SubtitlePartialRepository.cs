using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class SubtitlePartialRepository : ISubtitlePartialRepository
{
    private readonly DbSet<SubtitlePartial> _dbSet;

    public SubtitlePartialRepository(AlmanimeContext context) => _dbSet = context.Set<SubtitlePartial>();

    public SubtitlePartial? GetByID(Guid id) => _dbSet.SingleOrDefault(o => o.ID == id);
    public SubtitlePartial Create(SubtitlePartial entity) => _dbSet.Add(entity).Entity;
    public void Delete(Guid id) => Delete(GetByID(id));
    public void Delete(SubtitlePartial? subtitlePartial)
    {
        if (subtitlePartial != null) _dbSet.Remove(subtitlePartial);
    }
}
