using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private ReizenContext _context;
        private DbSet<TEntity> _dbSet;
        
        public GenericRepository(ReizenContext context)
        {
            _context = context; 
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<TEntity> Delete (int id)
        {
            var elem = await _dbSet.FindAsync (id);
            _dbSet.Remove (elem);
            return elem;
        }

        public TEntity Delete (TEntity entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAll ()
        {
            return (await _dbSet.ToListAsync<TEntity> ()).ToImmutableList();
        }

        public async Task<TEntity?> GetByID (int id)
        {
            return await _dbSet.FindAsync (id);
        }

        public async Task<TEntity> Insert (TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public TEntity Update (TEntity entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
