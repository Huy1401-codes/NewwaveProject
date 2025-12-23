using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public int UserId { get; private set; }
        public int RoleId { get; private set; }

        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;
    }
}
