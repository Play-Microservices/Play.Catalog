using Play.Catalog.Service.Entites;

namespace Play.Catalog.Service.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task CreateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(Guid id);
    Task UpdateAsync(TEntity entity);
}