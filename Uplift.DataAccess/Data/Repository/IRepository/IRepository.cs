﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
            string IncludeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string IncludeProperties = null
            );

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);

    }
}
