using System;
using System.Collections.Generic;

namespace BusinessServices.Contracts
{
    public interface IBaseService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid entityId);
        T Create(T entity);
        T Update(T entity);
        T Delete(Guid entityId);
    }
}
