using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Common.BaseEntities
{
    public abstract class AuditableEntity<T> : BaseEntity<T>
    {
        public DateTime CreatedAt { get;  set; }
        public DateTime? UpdatedAt { get;  set; }

        public void SetCreated() => CreatedAt = DateTime.UtcNow;
        public void SetUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}

