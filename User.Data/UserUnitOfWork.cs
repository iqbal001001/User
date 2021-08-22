using User.RepositoryInterface;
using System.Threading.Tasks;
using System.Threading;

namespace User.Data
{
    public class UserUnitOfWork : IUnitOfWork
    {
        private UserContext _context;

        protected UserContext Context
        {
            get { return _context ?? (_context = UserContextFactory.Get()); }
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

