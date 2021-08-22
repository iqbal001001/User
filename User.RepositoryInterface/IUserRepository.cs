using System;
using System.Collections.Generic;
using System.Text;
using User.Domain;

namespace User.RepositoryInterface
{
    public interface IUserRepository: IRepositoryBase<UserInfo>
    {
    }
}
