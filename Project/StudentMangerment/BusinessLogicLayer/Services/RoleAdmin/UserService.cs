using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        public UserService(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        /// <summary>
        /// Lấy danh sách người dùng có phân trang + lọc + search
        /// </summary>
        public async Task<(IEnumerable<UserListDto> Users, int Total)> GetPagedUsersAsync(
            string search, int pageIndex, int pageSize, int? roleId, bool? status)
        {
            var query = _userRepo.GetAllQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                query = query.Where(u =>
                    u.Username.Contains(search) ||
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search)
                );
            }

            // FILTER ROLE
            if (roleId.HasValue)
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId.Value));
            }

            // FILTER STATUS
            if (status.HasValue)
            {
                query = query.Where(u => u.IsStatus == status.Value);
            }

            // TOTAL BEFORE PAGING
            int total = await query.CountAsync();

            // APPLY PAGING + SELECT DTO
            var users = await query
                .OrderBy(u => u.Username)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    IsStatus = u.IsStatus,
                    Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList()
                })
                .ToListAsync();

            return (users, total);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepo.GetAllAsync();
        }

        /// <summary>
        /// Lấy user theo ID 
        /// </summary>
        public async Task<UserDetailDto> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null) return null;

            return new UserDetailDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = user.IsStatus,
                RoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList(),
                RoleNames = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };
        }


        /// <summary>
        /// Create user
        /// </summary>
        public async Task AddAsync(UserCreateDto dto)
        {
            if (dto.IsStatus == null)
                throw new ArgumentException("Vui lòng chọn trạng thái cho tài khoản");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                IsStatus = dto.IsStatus
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

            // Cập nhật role
            if (dto.RoleId > 0)
            {
                user.UserRoles = new List<UserRole>
    {
        new UserRole
        {
            UserId = user.UserId,
            RoleId = dto.RoleId
        }
    };

                await _userRepo.SaveAsync();
            }

        }


        /// <summary>
        /// Update user
        /// </summary>
        public async Task UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null) return;

            if (dto.IsStatus == null)
                throw new ArgumentException("Vui lòng chọn trạng thái cho tài khoản");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.IsStatus = dto.IsStatus;

            // Update roles
            user.UserRoles.Clear();
            // Chỉ thêm 1 role nếu được chọn
            if (dto.RoleId > 0)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.UserId,
                    RoleId = dto.RoleId
                });
            }

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();
        }


        /// <summary>
        /// Soft delete
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            await _userRepo.SoftDeleteAsync(id);
            await _userRepo.SaveAsync();
        }
    }


}

