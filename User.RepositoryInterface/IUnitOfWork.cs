using System.Threading;
using System.Threading.Tasks;

namespace User.RepositoryInterface
{
    public interface IUnitOfWork
    {
        public void SaveChanges();
        public void Dispose();

         Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
