using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ParkingSlot
{
    public class ParkingSlotDto
    {
        public int Id { get; set; }
        public string SlotCode { get; set; } = null!;
        public bool IsOccupied { get; set; }
    }
}
