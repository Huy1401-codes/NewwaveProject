using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Common.BaseEntities
{
    public abstract class ActivatableEntity<T> : AuditableEntity<T>
    {
        public bool IsActive { get; protected set; } = true;

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
