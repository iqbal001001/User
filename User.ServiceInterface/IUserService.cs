using System;
using System.Collections.Generic;
using User.Domain;
using System.Threading.Tasks;
using System.Threading;

namespace User.ServiceInterface
{
    public interface IUserService
    {
        Task<bool> AnyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Dictionary<string, string> filters, CancellationToken cancellationToken = default);
        Task<(List<UserInfo>, int)> GetAllAsync(string sort,
            int page, int pageSize, Dictionary<string, string> filter, CancellationToken cancellationToken = default);
        Task<UserInfo> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(UserInfo User, CancellationToken cancellationToken = default);
        Task UpdateAsync(UserInfo User, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        bool Validate(UserInfo User);
    }
}
