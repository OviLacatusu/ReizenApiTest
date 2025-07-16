using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAll();
        public Task<TEntity> GetByID(int id);
        public Task<TEntity> Insert(TEntity entity);
        public TEntity Update(TEntity entity);
        public Task<TEntity> Delete(int id);
        public TEntity Delete (TEntity entity);
    }
}
