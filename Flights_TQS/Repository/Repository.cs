using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Flights_TQS.Interfaces;

namespace Flights_TQS.Repository {
  public class Repository<TEntity>: IRepository<TEntity> where TEntity: class {
    private readonly ISession Session;

    public Repository(ISession session) {
      Session = session;
    }

    public TEntity this[object idx] {
      get { return Get(idx); }
    }

    public TEntity Get(object idx) {
      return Session.Get<TEntity>(idx);
    }

    public TEntity Get(Func<TEntity, Boolean> predicate) {
      return Session.Query<TEntity>().FirstOrDefault(predicate);
    }

    public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params string[] includes) {
      var query = Session.Query<TEntity>();

      foreach (var include in includes)
        query.FetchMany(e => e.GetType().GetProperty(include).Name);

      return query.Where(predicate);
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) {
      var query = Session.Query<TEntity>();

      if (predicate != null)
        query = query.Where(predicate);

      return query.ToList();
    }

    public IEnumerable<TEntity> GetAll() {
      return GetAll(null);
    }

    public void Add(TEntity entity) {
      Session.Save(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities) {
      foreach (TEntity entity in entities)
        Session.Save(entity);
    }

    public void Remove(object idx) {
      TEntity entity = Session.Get<TEntity>(idx);
      Session.Delete(entity);
    }

    public void Remove(TEntity entity) {
      Session.Delete(entity);
    }

    public void Remove(Expression<Func<TEntity, Boolean>> predicate) {
      IQueryable<TEntity> entities = Session.Query<TEntity>().Where(predicate).AsQueryable();
      foreach (TEntity entity in entities) Session.Delete(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities) {
      foreach (TEntity entity in entities)
        Session.Delete(entity);
    }

    public void Update(TEntity entity) {
      Session.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities) {
      foreach (TEntity entity in entities)
        Session.Update(entity);
    }

    public IQueryable<TEntity> AsQueryable() {
      return Session.Query<TEntity>();
    }

    public IQueryOver<TEntity, TEntity> AsQueryOver() {
      return Session.QueryOver<TEntity>();
    }

    public IQueryOver<TEntity, TEntity> AsQueryOver(Expression<Func<TEntity>> alias) {
      return Session.QueryOver<TEntity>(alias);
    }

    public IQueryOver<TEntity, TEntity> AsQueryOver(Expression<Func<TEntity, bool>> predicate) {
      return Session.QueryOver<TEntity>().Where(predicate);
    }

    public bool Exists(object primaryKey) {
      return (Get(primaryKey) != null);
    }

    public TEntity First(Expression<Func<TEntity, bool>> predicate) {
      return Session.Query<TEntity>().First(predicate);
    }

    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate) {
      return Session.Query<TEntity>().FirstOrDefault(predicate);
    }
  }
}