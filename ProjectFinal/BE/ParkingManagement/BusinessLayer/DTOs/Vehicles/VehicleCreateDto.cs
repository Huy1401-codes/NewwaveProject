using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Vehicles
{
    public class VehicleCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string PlateNumber { get; set; } = null!;

        [Required]
        public int VehicleTypeId { get; set; }
    }
}
