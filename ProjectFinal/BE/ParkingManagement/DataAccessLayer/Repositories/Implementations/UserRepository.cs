using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ParkingDbContext context) : base(context)
        {
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.Include(a=>a.UserRoles).ThenInclude(a=>a.Role)
                               .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
