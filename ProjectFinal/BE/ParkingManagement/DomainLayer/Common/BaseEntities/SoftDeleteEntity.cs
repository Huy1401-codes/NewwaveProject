using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Common.BaseEntities
{
    public abstract class SoftDeleteEntity<T> : ActivatableEntity<T>
    {
        public bool IsDeleted { get; protected set; }
        public DateTime? DeletedAt { get; protected set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
