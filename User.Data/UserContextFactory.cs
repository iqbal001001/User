using User.RepositoryInterface;

namespace User.Data
{
    public interface IDbContextFactory
    {
        UserContext Get();
    }
    public class UserContextFactory : IDbContextFactory
    {
            private UserContext _context;

            public UserContext Get()
            {
                if (_context == null) InitialiseContext();

                return _context;
            }

            public  void InitialiseContext()
            {
                _context = new UserContext();
            }
        }
}
