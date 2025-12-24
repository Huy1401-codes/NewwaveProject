using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Vehicle : SoftDeleteEntity<int>
    {
        public string PlateNumber { get;  set; } = null!;
        public int VehicleTypeId { get;  set; }
        public int? OwnerId { get;  set; }

        public VehicleType VehicleType { get;  set; } = null!;
        public User? Owner { get;  set; }

        public ICollection<ParkingRecord> ParkingRecords { get; private set; } = new List<ParkingRecord>();
        public ICollection<MonthlyPass> MonthlyPasses { get; private set; } = new List<MonthlyPass>();
    }
}
