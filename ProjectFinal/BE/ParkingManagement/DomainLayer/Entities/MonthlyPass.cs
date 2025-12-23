using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class MonthlyPass : ActivatableEntity<int>
    {
        public int VehicleId { get; private set; }
        public int UserId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal MonthlyPrice { get; private set; }

        public Vehicle Vehicle { get; private set; } = null!;
        public User User { get; private set; } = null!;
    }
}
