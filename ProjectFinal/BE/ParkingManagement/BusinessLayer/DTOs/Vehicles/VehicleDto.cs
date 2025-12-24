using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Vehicles
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = null!;
        public int VehicleTypeId { get; set; }
    }
}
