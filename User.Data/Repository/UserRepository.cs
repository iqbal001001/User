using User.Domain;
using User.RepositoryInterface;

namespace User.Data.Repository
{
    public class UserRepository : RepositoryBase<UserInfo>, IUserRepository
    {
        public UserRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }
}
