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
        public string TokenHash { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }

        public string CreatedByIp { get; private set; } = null!;
        public DateTime? RevokedAt { get; private set; }
        public string? RevokedByIp { get; private set; }
        public string? ReplacedByTokenHash { get; private set; }

        public int UserId { get; private set; }
        public User User { get; private set; } = null!;
    }
}
