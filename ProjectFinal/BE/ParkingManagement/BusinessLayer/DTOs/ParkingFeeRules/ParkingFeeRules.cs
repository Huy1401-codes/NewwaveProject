using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ParkingFeeRules
{
    public class ParkingFeeRuleDto
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public decimal PricePerHour { get; set; }
        public bool IsActive { get; set; }
    }
}
