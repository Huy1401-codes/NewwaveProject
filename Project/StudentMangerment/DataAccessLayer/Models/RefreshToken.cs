using DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RefreshToken : BaseEntity
    {
        public string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByTokenHash { get; set; }

        public bool IsActive =>
            RevokedAt == null && ExpiresAt > DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }
    }

}
