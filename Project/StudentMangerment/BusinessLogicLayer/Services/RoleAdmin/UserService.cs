using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
                RoleIds = user.UserRoles.FirstOrDefault()?.RoleId ?? 0,
                RoleNames = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };
        }


        /// <summary>
        /// Create user
        /// </summary>
        public async Task<(bool Success, List<string> Errors)> AddAsync(UserCreateDto dto)
        {
            var errors = new List<string>();

            // Validate tất cả field
            if (string.IsNullOrWhiteSpace(dto.Username))
                errors.Add("Tên đăng nhập không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Mật khẩu không được để trống.");
            else if (!Regex.IsMatch(dto.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
                errors.Add("Mật khẩu phải ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                errors.Add("Họ và tên không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email không được để trống.");
            else if (!Regex.IsMatch(dto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                errors.Add("Email không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                errors.Add("Số điện thoại không được để trống.");
            else if (!Regex.IsMatch(dto.Phone, @"^\d{10}$"))
                errors.Add("Số điện thoại phải gồm 10 chữ số.");

            if (dto.IsStatus == null)
                errors.Add("Vui lòng chọn trạng thái cho tài khoản.");

            if (dto.RoleId <= 0)
                errors.Add("Vui lòng chọn vai trò cho tài khoản.");

            //  Nếu có lỗi cơ bản thì không cần kiểm duplicate
            if (errors.Any())
                return (false, errors);


            // Check email
            var existingEmail = await _userRepo.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (existingEmail != null)
                errors.Add("Email đã tồn tại trong hệ thống.");

            // Check phone
            var existingPhone = await _userRepo.FirstOrDefaultAsync(x => x.Phone == dto.Phone);
            if (existingPhone != null)
                errors.Add("Số điện thoại đã tồn tại trong hệ thống.");

            if (errors.Any())
                return (false, errors);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                IsStatus = dto.IsStatus.Value
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

            user.UserRoles = new List<UserRole>
    {
        new UserRole
        {
            UserId = user.UserId,
            RoleId = dto.RoleId
        }
    };

            await _userRepo.SaveAsync();

            return (true, null);
        }




        /// <summary>
        /// Update user
        /// </summary>
        public async Task<(bool Success, List<string> Errors)> UpdateAsync(UserUpdateDto dto)
        {
            var errors = new List<string>();

            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                errors.Add("Người dùng không tồn tại.");
                return (false, errors);
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
                errors.Add("Họ và tên không được để trống.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email không được để trống.");
            else if (!Regex.IsMatch(dto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                errors.Add("Email không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.Phone))
                errors.Add("Số điện thoại không được để trống.");
            else if (!Regex.IsMatch(dto.Phone, @"^\d{10}$"))
                errors.Add("Số điện thoại phải gồm 10 chữ số.");

            if (dto.IsStatus == null)
                errors.Add("Vui lòng chọn trạng thái cho tài khoản.");

            if (dto.RoleId <= 0)
                errors.Add("Vui lòng chọn vai trò.");

            if (errors.Any())
                return (false, errors);

            // Duplicate Phone 
            var phoneExists = await _userRepo.FirstOrDefaultAsync(x => x.Phone == dto.Phone && x.UserId != dto.UserId);
            if (phoneExists != null)
                errors.Add("Số điện thoại đã tồn tại trong hệ thống.");

            // Duplicate Username nếu có thay đổi Username trong UpdateDto
            if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
            {
                var usernameExists = await _userRepo.FirstOrDefaultAsync(x => x.Username == dto.Username && x.UserId != dto.UserId);
                if (usernameExists != null)
                    errors.Add("Tên đăng nhập đã tồn tại.");
            }

            if (errors.Any())
                return (false, errors);


            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.IsStatus = dto.IsStatus.Value;

            // Nếu update Username (nếu cho phép)
            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;

            // === Update Role ===
            user.UserRoles.Clear();
            user.UserRoles.Add(new UserRole
            {
                UserId = user.UserId,
                RoleId = dto.RoleId
            });

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();

            return (true, null);
        }


        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            //mã hóa mật khâu bằng thư viện 
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();

            return true;
        }


        /// <summary>
        /// Soft delete
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            await _userRepo.SoftDeleteAsync(id);
            await _userRepo.SaveAsync();
        }

        public async Task<(IEnumerable<UserListDto> Users, int Total)> GetRestoreUsersAsync(string search, int pageIndex, int pageSize, int? roleId)
        {
            var query = _userRepo.GetAllRestoreQueryable();

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
    }


}

