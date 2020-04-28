using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contracts
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> GetAll();
        T GetById<TKey>(TKey key);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
        int SaveChanges();
    }
}
