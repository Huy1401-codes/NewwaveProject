using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class VehicleType : BaseEntity<int>
    {
        public string Name { get; private set; } = null!;

        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        public ICollection<ParkingFeeRule> ParkingFeeRules { get; private set; } = new List<ParkingFeeRule>();
    }
}
