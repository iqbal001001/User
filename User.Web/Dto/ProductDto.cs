using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Web.Dto
{
    public class UserDto : UserInsertDto
    {
        public int  Id { get; set; }
    }

    public class UserInsertDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }

    }


}
