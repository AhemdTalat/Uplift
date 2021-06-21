using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.DbSet = context.Set<T>();
        }


        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string IncludeProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = DbSet.Where(filter);
            }
            if (IncludeProperties != null)
            {
                foreach(var IncludeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludeProperty);
                }
            }

            if (OrderBy != null)
            {
                return OrderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string IncludeProperties = null)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = DbSet.Where(filter);
            }
            if (IncludeProperties != null)
            {
                foreach (var IncludeProperty in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            var EntityFromDb = DbSet.Find(id);
            Remove(EntityFromDb);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
