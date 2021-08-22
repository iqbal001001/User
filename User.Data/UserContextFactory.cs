using User.RepositoryInterface;

namespace User.Data
{
        public class UserContextFactory 
    {
            private static UserContext _context;

            public static UserContext Get()
            {
                if (_context == null) InitialiseContext();

                return _context;
            }

            public  static void InitialiseContext()
            {
                _context = new UserContext();
            }
        }
}
