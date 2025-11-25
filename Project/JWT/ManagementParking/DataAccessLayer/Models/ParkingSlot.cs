using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ParkingSlot
    {
        public int ParkingSlotId { get; set; }

        [Required, MaxLength(50)]
        public string SlotCode { get; set; }

        public bool IsOccupied { get; set; } = false;
        public string Description { get; set; }

        public ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

}
