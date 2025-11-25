using BusinessLogicLayer.Services.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<(IEnumerable<User> Users, int Total)> GetPagedUsersAsync(
         string search, int pageIndex, int pageSize, int? roleId, bool? status)
        {
            var query = _userRepo.GetAllQueryable();

            // Search 
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(u =>
                    u.Username.Contains(search) ||
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Phone.Contains(search)
                );
            }

            // Filter role
            if (roleId.HasValue)
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId.Value));
            }

            // Filter status
            if (status.HasValue)
            {
                query = query.Where(u => u.IsStatus == status.Value);
            }

            // Count before paging
            int total = await query.CountAsync();

            // Get data with paging
            var users = await query
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .OrderBy(u => u.Username)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, total);
        }


        public async Task<User> GetByIdAsync(int id) => await _userRepo.GetByIdAsync(id);

        public async Task AddAsync(User user) => await _userRepo.AddAsync(user);

        public async Task UpdateAsync(User user) => await _userRepo.UpdateAsync(user);

        public async Task DeleteAsync(int id) => await _userRepo.SoftDeleteAsync(id);
    }

}

