using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ParkingFeeRule
    {
        public int ParkingFeeRuleId { get; set; }

        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        public int BlockMinutes { get; set; } = 60;
        public decimal PricePerBlock { get; set; }
        public decimal? MaxPricePerDay { get; set; }
        public decimal? MonthlyPrice { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    }

}
