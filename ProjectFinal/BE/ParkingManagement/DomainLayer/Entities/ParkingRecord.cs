using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ParkingRecord : AuditableEntity<int>
    {
        public int VehicleId { get; private set; }
        public int? ParkingSlotId { get; private set; }

        public DateTime TimeIn { get; private set; }
        public DateTime? TimeOut { get; private set; }
        public decimal? Fee { get; private set; }
        public string Note { get; private set; } = null!;

        public Vehicle Vehicle { get; private set; } = null!;
        public ParkingSlot? ParkingSlot { get; private set; }
    }
}
