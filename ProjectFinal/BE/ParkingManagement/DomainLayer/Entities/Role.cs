using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Role : BaseEntity<int>
    {
        public string RoleName { get; private set; } = null!;

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    }
}
