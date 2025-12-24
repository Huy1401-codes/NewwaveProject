using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ParkingSlot : SoftDeleteEntity<int>
    {
        public string SlotCode { get;  set; } = null!;
        public bool IsOccupied { get;  set; }
        public string Description { get;  set; } = null!;

        public ICollection<ParkingRecord> ParkingRecords { get; private set; } = new List<ParkingRecord>();
    }
}
