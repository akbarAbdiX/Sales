using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BigBang.Metadata.Models;

namespace BigBang.App.Sales.Repository.Base
{
    public interface IRepository<T> where T : IEntity
    {
        #region Commands
        T Insert(T entity);
        T Update(T entity);
        void Delete(T entity);
        void BulkInsert(IEnumerable<T> entities);
        void BulkUpdate(IEnumerable<T> entities);
        void BulkDelete(IEnumerable<T> entities);
        #endregion

        #region Queries
        T Load(object id);
        T Get(object id);
        T GetSingleOrDefault(Expression<Func<T, bool>> conditions);
        TResult GetSingleOrDefault<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection);
        T GetFirstOrDefault(Expression<Func<T, bool>> conditions);
        T GetFirstOrDefaultOrdered<TKey>(Expression<Func<T, bool>> conditions, Func<T, TKey> keySelector);
        T GetLastOrDefaultOrdered<TKey>(Expression<Func<T, bool>> conditions, Func<T, TKey> keySelector);
        TResult GetFirstOrDefault<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection);
        IList<T> GetMany(Expression<Func<T, bool>> conditions);
        IList<TResult> GetMany<TResult>(Expression<Func<T, bool>> conditions, Expression<Func<T, TResult>> projection);
        IList<TResult> GetMany<TResult>(Expression<Func<T, TResult>> projection);
        bool IsExists(Func<T, bool> condition);
        IList<T> GetMany();
        #endregion
    }
}
