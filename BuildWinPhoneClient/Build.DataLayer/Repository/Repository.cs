#region Using Directives

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

using Build.DataLayer.Context;
using Build.DataLayer.Interfaces;

#endregion

namespace Build.DataLayer.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly BuildContext context = new BuildContext();

        private Table<TEntity> dataTable;

        #endregion

        #region Constructors and Destructors

        public Repository()
        {
            this.dataTable = this.context.GetTable<TEntity>();
        }

        #endregion

        #region Public Methods and Operators

        public void Delete(TEntity entity)
        {
            this.dataTable.DeleteOnSubmit(entity);
        }

        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.DeleteAllOnSubmit(entities);
        }

        public TEntity FirstOrDefault()
        {
            return this.dataTable.FirstOrDefault();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataTable.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.dataTable;
        }

        public void Insert(TEntity entity)
        {
            this.dataTable.InsertOnSubmit(entity);
        }

        public void InsertAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.InsertAllOnSubmit(entities);
        }

        public void SubmitChanges()
        {
            this.context.SubmitChanges();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataTable.Where(predicate);
        }

        #endregion
    }
}