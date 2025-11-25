using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ParkingRecord
    {
        public int ParkingRecordId { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int? ParkingSlotId { get; set; }
        public ParkingSlot ParkingSlot { get; set; }

        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        public decimal? Fee { get; set; }
        public string Note { get; set; }
    }


}
