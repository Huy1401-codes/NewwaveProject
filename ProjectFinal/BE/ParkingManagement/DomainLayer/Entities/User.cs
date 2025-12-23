using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class User : ActivatableEntity<int>
    {
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Phone { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string PasswordSalt { get; private set; } = null!;

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        public ICollection<MonthlyPass> MonthlyPasses { get; private set; } = new List<MonthlyPass>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    }
}
