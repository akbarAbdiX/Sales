using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BigBang.Metadata.Models;
using BigBang.WebServer.Common;
using BigBang.WebServer.Common.DataAccess;
using NHibernate;

namespace BigBang.App.Sales.Repository.Base
{
    internal abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        #region Commands

        public T Update(T entity)
        {
            _session.Update(entity);
            _session.Flush();
            return entity;
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
            _session.Flush();
        }

        public T Insert(T entity)
        {
            _session.Save(entity);
            _session.Flush();
            return entity;
        }

        public virtual void BulkInsert(IEnumerable<T> entities)
        {
            BulkOperation(entities, EntityState.Added);
        }

        public virtual void BulkUpdate(IEnumerable<T> entities)
        {
            BulkOperation(entities, EntityState.Modified);
        }

        public virtual void BulkDelete(IEnumerable<T> entities)
        {
            BulkOperation(entities, EntityState.Deleted);
        }

        private void BulkOperation(IEnumerable<T> entities, EntityState state)
        {
            var entityList = entities as List<T> ?? entities.ToList();
            entityList.ForEach(e => e.EntityState = state);
            ServiceLocator.Get<IBulkRepository<T>>().Save(entityList);
        }


        #endregion

        #region Queries
        public T Get(object id)
        {
            return _session.Get<T>(id);
        }
        public T GetSingleOrDefault(Expression<Func<T, bool>> conditions)
        {
            return _session.Query<T>().SingleOrDefault(conditions);
        }
        public TResult GetSingleOrDefault<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection)
        {
            return _session.Query<T>()
                .Where(conditions)
                .Select(projection)
                .SingleOrDefault();
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> conditions)
        {
            return _session.Query<T>().FirstOrDefault(conditions);
        }

        public T GetFirstOrDefaultOrdered<TKey>(Expression<Func<T, bool>> conditions, Func<T, TKey> keySelector)
        {
            return _session.Query<T>()
                .Where(conditions)
                .AsEnumerable()
                .OrderBy(keySelector)
                .FirstOrDefault();
        }

        public T GetLastOrDefaultOrdered<TKey>(Expression<Func<T, bool>> conditions, Func<T, TKey> keySelector)
        {
            return _session.Query<T>()
                .Where(conditions)
                .AsEnumerable()
                .OrderByDescending(keySelector)
                .FirstOrDefault();
        }

        public TResult GetFirstOrDefault<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection)
        {
            return _session.Query<T>()
                .Where(conditions)
                .Select(projection)
                .FirstOrDefault();
        }
        public IList<T> GetMany()
        {
            return _session.Query<T>().ToList();
        }

        public IList<T> GetMany(Expression<Func<T, bool>> conditions)
        {
            return _session.Query<T>().Where(conditions).ToList();
        }
        public IList<TResult> GetMany<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection)
        {
            return _session.Query<T>().Where(conditions).Select(projection).ToList();
        }
        public IList<TResult> GetMany<TResult>(Expression<Func<T, TResult>> projection)
        {
            return _session.Query<T>().Select(projection).ToList();
        }
        public bool IsExists(Func<T, bool> condition)
        {
            return _session.Query<T>().Any(condition);
        }
        public T Load(object id)
        {
            return _session.Load<T>(id);
        }
        #endregion
    }
}
