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
        public string SlotCode { get; private set; } = null!;
        public bool IsOccupied { get; private set; }
        public string Description { get; private set; } = null!;

        public ICollection<ParkingRecord> ParkingRecords { get; private set; } = new List<ParkingRecord>();
    }
}
