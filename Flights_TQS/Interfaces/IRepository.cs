using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;

namespace Flights_TQS.Interfaces {
  public interface IRepository<TEntity> where TEntity: class {
    TEntity this[object idx] { get; }
    TEntity Get(object idx);
    TEntity Get(Func<TEntity, bool> predicate);
    IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params string[] includes);
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetAll();
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(object idx);
    void Remove(TEntity entity);
    void Remove(Expression<Func<TEntity, bool>> predicate);
    void RemoveRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    bool Exists(object primaryKey);
    IQueryable<TEntity> AsQueryable();
    IQueryOver<TEntity, TEntity> AsQueryOver();
    IQueryOver<TEntity, TEntity> AsQueryOver(Expression<Func<TEntity>> alias);
    IQueryOver<TEntity, TEntity> AsQueryOver(Expression<Func<TEntity, bool>> predicate);
    TEntity First(Expression<Func<TEntity, bool>> predicate);
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
  }
}