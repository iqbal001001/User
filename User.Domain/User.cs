using System;

namespace User.Domain
{
    public class UserInfo : Data
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public bool Status { get; set; }
    }

    public enum GenderType
    {
        Male =1, Female=2, Other=3, 
    }
}
