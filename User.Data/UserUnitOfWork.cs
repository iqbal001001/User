using User.RepositoryInterface;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace User.Data
{
    public class UserUnitOfWork : IUnitOfWork
    {
        private IDbContextFactory _contextFactory;
        private UserContext _context;

        public UserUnitOfWork(IDbContextFactory contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            _contextFactory = contextFactory;
        }
        protected UserContext Context
        {
            get { return _context ?? (_context = _contextFactory.Get()); }
        }

        public void SaveChanges()
        {
            //Context.Commit();
            Context.SaveChanges();
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Context.Commit();
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

