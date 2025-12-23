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
        public string PlateNumber { get; private set; } = null!;
        public int VehicleTypeId { get; private set; }
        public int? OwnerId { get; private set; }

        public VehicleType VehicleType { get; private set; } = null!;
        public User? Owner { get; private set; }

        public ICollection<ParkingRecord> ParkingRecords { get; private set; } = new List<ParkingRecord>();
        public ICollection<MonthlyPass> MonthlyPasses { get; private set; } = new List<MonthlyPass>();
    }
}
