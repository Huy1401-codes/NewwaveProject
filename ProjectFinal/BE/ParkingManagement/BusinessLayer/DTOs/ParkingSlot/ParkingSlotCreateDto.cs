using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.ParkingSlot
{

    public class ParkingSlotCreateDto
    {
        [Required]
        public string SlotCode { get; set; } = null!;
    }
}
