using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class VehicleType
    {
        public int VehicleTypeId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<ParkingFeeRule> ParkingFeeRules { get; set; }
    }

}
