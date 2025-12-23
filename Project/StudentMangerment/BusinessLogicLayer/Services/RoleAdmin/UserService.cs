using BusinessLogicLayer.DTOs.Admin;
using BusinessLogicLayer.Messages.Admin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Services.RoleAdmin
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        #region Get Paged Users
        public async Task<(IEnumerable<UserListDto> Users, int Total)> GetPagedUsersAsync(
            string search, int pageIndex, int pageSize, int? roleId, bool? status)
        {
            try
            {
                var query = _unitOfWork.Users.Query();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(u =>
                        u.Username.ToLower().Contains(search) ||
                        u.FullName.ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search));
                }

                if (roleId.HasValue)
                    query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId.Value));

                if (status.HasValue)
                    query = query.Where(u => u.IsStatus == status.Value);

                int total = await query.CountAsync();

                var users = await query
                    .OrderBy(u => u.Username)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserListDto
                    {
                        UserId = u.Id,
                        Username = u.Username,
                        FullName = u.FullName,
                        Email = u.Email,
                        Phone = u.Phone,
                        IsStatus = u.IsStatus,
                        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                    })
                    .ToListAsync();

                return (users, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }
        #endregion

        #region Get User by Id
        public async Task<UserDetailDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning(UserMessages.UserNotFound, id);
                return null;
            }

            return new UserDetailDto
            {
                UserId = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsStatus = user.IsStatus,
                RoleIds = user.UserRoles.FirstOrDefault()?.RoleId ?? 0,
                RoleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }
        #endregion

        #region Add User
        public async Task<(bool Success, List<string> Errors)> AddAsync(UserCreateDto dto)
        {
            var errors = new List<string>();

            try
            {
                ValidateUserCreate(dto, errors);
                if (errors.Any()) return (false, errors);

                if (await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Email == dto.Email) != null)
                    errors.Add(UserMessages.EmailExists);

                if (await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Phone == dto.Phone) != null)
                    errors.Add(UserMessages.PhoneExists);

                if (errors.Any())
                {
                    _logger.LogWarning(UserMessages.Dulicate);
                    return (false, errors);
                }

                var user = new User
                {
                    Username = dto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    IsStatus = dto.IsStatus.Value
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveAsync();

                user.UserRoles = new List<UserRole>
                {
                    new UserRole { UserId = user.Id, RoleId = dto.RoleId }
                };

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(UserMessages.CreateSuccess);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }

        private void ValidateUserCreate(UserCreateDto dto, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(dto.Username)) errors.Add(UserMessages.UsernameRequired);
            if (string.IsNullOrWhiteSpace(dto.Password)) errors.Add(UserMessages.PasswordRequired);
            else if (!Regex.IsMatch(dto.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
                errors.Add(UserMessages.PasswordInvalid);

            if (string.IsNullOrWhiteSpace(dto.FullName)) errors.Add(UserMessages.FullNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Email)) errors.Add(UserMessages.EmailRequired);
            else if (!Regex.IsMatch(dto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$")) errors.Add(UserMessages.EmailInvalid);

            if (string.IsNullOrWhiteSpace(dto.Phone)) errors.Add(UserMessages.PhoneRequired);
            else if (!Regex.IsMatch(dto.Phone, @"^\d{10}$")) errors.Add(UserMessages.PhoneInvalid);

            if (dto.IsStatus == null) errors.Add(UserMessages.StatusRequired);
            if (dto.RoleId <= 0) errors.Add(UserMessages.RoleRequired);
        }
        #endregion

        #region Update User
        public async Task<(bool Success, List<string> Errors)> UpdateAsync(UserUpdateDto dto)
        {
            var errors = new List<string>();

            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                if (user == null) { errors.Add(UserMessages.UserNotFound); return (false, errors); }

                ValidateUserUpdate(dto, errors);
                if (errors.Any()) return (false, errors);

                if (await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Phone == dto.Phone && x.Id != dto.UserId) != null)
                    errors.Add(UserMessages.PhoneExists);

                if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
                {
                    if (await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Username == dto.Username) != null)
                        errors.Add(UserMessages.UsernameExists);
                }

                if (errors.Any()) return (false, errors);

                user.FullName = dto.FullName;
                user.Phone = dto.Phone;
                user.IsStatus = dto.IsStatus.Value;
                if (!string.IsNullOrEmpty(dto.Username)) user.Username = dto.Username;

                var currentRole = user.UserRoles.FirstOrDefault();
                if (currentRole != null) currentRole.RoleId = dto.RoleId;
                else user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = dto.RoleId });

                await _unitOfWork.SaveAsync();

                _logger.LogInformation(UserMessages.UpdateSuccess);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }

        private void ValidateUserUpdate(UserUpdateDto dto, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName)) errors.Add(UserMessages.FullNameRequired);
            if (string.IsNullOrWhiteSpace(dto.Email)) errors.Add(UserMessages.EmailRequired);
            else if (!Regex.IsMatch(dto.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$")) errors.Add(UserMessages.EmailInvalid);

            if (string.IsNullOrWhiteSpace(dto.Phone)) errors.Add(UserMessages.PhoneRequired);
            else if (!Regex.IsMatch(dto.Phone, @"^\d{10}$")) errors.Add(UserMessages.PhoneInvalid);

            if (dto.IsStatus == null) errors.Add(UserMessages.StatusRequired);
            if (dto.RoleId <= 0) errors.Add(UserMessages.RoleRequired);
        }
        #endregion

        #region Reset Password
        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning(UserMessages.UserNotFound, userId);
                    return false;
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(UserMessages.Reset, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }
        #endregion

        #region Delete / Restore
        public async Task DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.Users.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation(UserMessages.DeleteSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }

        public async Task<(IEnumerable<UserListDto> Users, int Total)> GetRestoreUsersAsync(
            string search, int pageIndex, int pageSize, int? roleId)
        {
            try
            {
                var query = _unitOfWork.Users.Query(); 

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(u =>
                        u.Username.ToLower().Contains(search) ||
                        u.FullName.ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search));
                }

                if (roleId.HasValue)
                    query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId.Value));

                int total = await query.CountAsync();

                var users = await query
                    .OrderBy(u => u.Username)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UserListDto
                    {
                        UserId = u.Id,
                        Username = u.Username,
                        FullName = u.FullName,
                        Email = u.Email,
                        Phone = u.Phone,
                        IsStatus = u.IsStatus,
                        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                    })
                    .ToListAsync();

                return (users, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }
        #endregion

        #region Get All Roles
        public async Task<List<Role>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                return roles.ToList(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }


        #endregion


        #region Get All Users
        public async Task<List<Role>> GetAllAsync()
        {
            try
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();
                return roles.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, UserMessages.UnexpectedError);
                throw;
            }
        }

        #endregion

    }
}
