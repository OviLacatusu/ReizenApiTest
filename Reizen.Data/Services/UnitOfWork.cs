using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Services
{
    public class UnitOfWork<TEntity>: IDisposable where TEntity : class
    {
        private ReizenContext _context;
        private GenericRepository<TEntity> genericRepository;

        public UnitOfWork (ReizenContext context)
        {
            _context = context;
        }
        public GenericRepository<TEntity> Repository
        {
            get
            {
                if (genericRepository == null)
                {
                    this.genericRepository = new GenericRepository<TEntity> (_context);
                }
                return genericRepository;
            }
        }

        public void Dispose ()
        {
            this._context.Dispose ();
        }

        public async Task SaveAsync ()
        {
            await this._context.SaveChangesAsync ();
        }

        
    }
}
