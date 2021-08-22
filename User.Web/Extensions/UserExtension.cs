using User.Web.Dto;
using System;
using User.Domain;

namespace User.Web.Extensions
{
    public static class UserExtension
    {
        public static UserDto ToDto(this UserInfo user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = Enum.GetName(typeof(GenderType), user.Gender),
                Status = user.Status
            };
        }

        public static UserInsertDto ToInsertDto(this UserInfo user)
        {
            return new UserInsertDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = Enum.GetName(typeof(GenderType), user.Gender),
                Status = user.Status
            };
        }


    }


    public static class UserDtoExtension
    {
        public static UserInfo ToDomain(this UserDto User, UserInfo originalUser = null)
        {
            if (originalUser != null)
            {
                originalUser.FirstName = User.FirstName;
                originalUser.LastName = User.LastName;
                originalUser.Email = User.Email;
                originalUser.Gender = (GenderType)Enum.Parse(typeof(GenderType), User.Gender, true);
                originalUser.Status = User.Status;
                return originalUser;
            }

            return new UserInfo
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Gender = (GenderType)Enum.Parse(typeof(GenderType), User.Gender, true),
                Status = User.Status
            };
        }

        public static UserInfo ToDomain(this UserInsertDto User, UserInfo originalUser = null)
        {
            if (originalUser != null)
            {
                originalUser.FirstName = User.FirstName;
                originalUser.LastName = User.LastName;
                originalUser.Email = User.Email;
                originalUser.Gender = (GenderType)Enum.Parse(typeof(GenderType), User.Gender, true);
                originalUser.Status = User.Status;

                return originalUser;
            }

            return new UserInfo
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Gender = (GenderType)Enum.Parse(typeof(GenderType), User.Gender, true),
                Status = User.Status
            };
        }


    }
}
