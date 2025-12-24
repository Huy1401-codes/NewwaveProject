using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class RefreshToken : AuditableEntity<int>
    {
        public string TokenHash { get;  set; } = null!;
        public DateTime ExpiresAt { get;  set; }

        public string CreatedByIp { get;  set; } = null!;
        public DateTime? RevokedAt { get;  set; }
        public string? RevokedByIp { get;  set; }
        public string? ReplacedByTokenHash { get;  set; }

        public int UserId { get;  set; }
        public User User { get;  set; } = null!;
    }
}
