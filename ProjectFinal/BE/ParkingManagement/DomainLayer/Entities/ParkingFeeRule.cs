using DomainLayer.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ParkingFeeRule : ActivatableEntity<int>
    {
        public int VehicleTypeId { get; private set; }

        public int BlockMinutes { get; private set; }
        public decimal PricePerBlock { get; private set; }
        public decimal? MaxPricePerDay { get; private set; }
        public decimal? MonthlyPrice { get; private set; }
        public DateTime EffectiveFrom { get; private set; }

        public VehicleType VehicleType { get; private set; } = null!;
    }
}
