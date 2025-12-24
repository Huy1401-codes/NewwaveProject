using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ParkingFeeRules
{
    public class ParkingFeeRuleUpdateDto
    {
        [Required]
        [Range(0, 1000000)]
        public decimal PricePerHour { get; set; }

        public bool IsActive { get; set; }
    }
}
