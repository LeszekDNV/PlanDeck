using System.Linq.Expressions;
using PlanDeck.Domain.ValueObjects;

namespace PlanDeck.Application.Interfaces;

public interface IRepository<T>
{
    void Add(T entity);
    Task AddAsync(T entity);

    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);
    Task UpdateAsync(T entity);

    void Delete(T entity);
    Task DeleteAsync(T entity);

    void RemoveRange(IEnumerable<T> entities);
    Task RemoveRangeAsync(IEnumerable<T> entities);

    IQueryable<T> GetAll();

    IQueryable<T> Find(Expression<Func<T, bool>> predicate);

    Task ApplyPatchAsync(T entity, params Patch[] patches);
}