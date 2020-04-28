using DAL;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;

namespace Repository.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private DataContext _dataContext;
        private DbSet<T> _dbSet;
        private DbQuery<T> _dbQuery;
        private bool _isDbSet;

        public RepositoryBase(DataContext dataContext, bool isDbSet = true)
        {
            _dataContext = dataContext;

            _isDbSet = isDbSet;
            if (_isDbSet)
            {
                _dbSet = _dataContext.Set<T>();
            }
            else
            {
                _dbQuery = _dataContext.Query<T>();
            }
        }

        public T Create(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public T Delete(T entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public T GetById<TKey>(TKey key)
        {
            var entity = _dbSet.Find(key);
            _dataContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public T Update(T entity)
        {
            return _dbSet.Update(entity).Entity;
        }
    }
}