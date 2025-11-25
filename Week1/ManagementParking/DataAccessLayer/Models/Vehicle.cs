using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        [Required, MaxLength(50)]
        public string PlateNumber { get; set; }

        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }

        public int? OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<ParkingRecord> ParkingHistory { get; set; }
        public ICollection<MonthlyPass> MonthlyPasses { get; set; }
    }


}
