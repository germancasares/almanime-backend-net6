using API.Models;

namespace API.Repositories.Interfaces;

public interface IBaseRepository<TModel> where TModel : Base
{
    IQueryable<TModel> GetAll();
    TModel? GetByID(Guid id);

    TModel Create(TModel entity);
    void Update(TModel entity);
    void BulkMerge(IEnumerable<TModel> entities);
    void Delete(Guid id);
    void Delete(TModel entity);
}
