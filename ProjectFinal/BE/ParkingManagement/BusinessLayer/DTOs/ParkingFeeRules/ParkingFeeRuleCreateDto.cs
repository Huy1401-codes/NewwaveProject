using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ParkingFeeRules
{
    public class ParkingFeeRuleCreateDto
    {
        [Required]
        public int VehicleTypeId { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal PricePerHour { get; set; }
    }

}
