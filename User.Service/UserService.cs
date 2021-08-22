using System;
using User.ServiceInterface;
using User.RepositoryInterface;
using User.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using LinqKit;
using User.Service.Extensions;

namespace Produt.Service
{
    public class UserService : IUserService
    {
        private  IUserRepository _UserRepo { get; set; }
        private  IUnitOfWork _unitOfWork;

        public UserService(IUserRepository UserRepo, IUnitOfWork unitOfWork)
        {
            _UserRepo = UserRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountAsync(Dictionary<string, string> filters = default, CancellationToken cancellationToken = default)
        {
            return await _UserRepo.CountAsync(filters.GetExpression<UserInfo>(), cancellationToken);
        }
        public async Task<(List<UserInfo>, int)> GetAllAsync(
            string sort = "Id",
            int page = 1, int pageSize = 5,
            Dictionary<string, string> filters = default,
            CancellationToken cancellationToken = default)
        {
            //var x = await _UserRepo.GetAsync(x=>x.Id..eq)
            var result =  await _UserRepo.GetAsync(sort, (page - 1) * pageSize, pageSize, filters.GetExpression<UserInfo>(), cancellationToken);
            var totalCount = await _UserRepo.CountAsync(filters.GetExpression<UserInfo>(), cancellationToken);
            return (result, totalCount);
        }

        public async Task<UserInfo> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _UserRepo.GetByIdAsync(id);
        }
        public async Task<bool> AnyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _UserRepo.AnyAsync(p => p.Id == id);
        }

        public async Task AddAsync(UserInfo User, CancellationToken cancellationToken = default)

        {
             await _UserRepo.AddAsync(User, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserInfo User, CancellationToken cancellationToken = default)
        {
             _UserRepo.Update(User);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public bool Validate(UserInfo User)
        {
            if (string.IsNullOrEmpty(User.FirstName) )
                return false;
            if (string.IsNullOrEmpty(User.LastName))
                return false;
            if (string.IsNullOrEmpty(User.Email))
                return false;

            return true;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _UserRepo.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
