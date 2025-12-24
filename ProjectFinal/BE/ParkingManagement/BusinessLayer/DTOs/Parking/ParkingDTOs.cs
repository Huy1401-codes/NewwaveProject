using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Parking
{
    public class ParkingDTOs
    {
        public class VehicleInDto
        {
            [Required]
            [MaxLength(20)]
            public string PlateNumber { get; set; } = null!;

            [Required]
            public int VehicleTypeId { get; set; }
        }


        public class VehicleOutDto
        {
            [Required]
            [MaxLength(20)]
            public string PlateNumber { get; set; } = null!;

            [Required]
            public PaymentMethod PaymentMethod { get; set; }
        }

        public class PaymentTransactionDto
        {
            public decimal Amount { get; set; }

            public string PaymentMethod { get; set; } = null!;

            public string Status { get; set; } = null!;

            public DateTime CreatedAt { get; set; }
        }

        public class ParkingRecordDto
        {
            public int Id { get; set; }

            public string PlateNumber { get; set; } = null!;

            public string VehicleType { get; set; } = null!;

            public string? ParkingSlotCode { get; set; }

            public DateTime TimeIn { get; set; }

            public DateTime? TimeOut { get; set; }

            public decimal? Fee { get; set; }

            public string? Note { get; set; }
        }

        public class MonthlyPassDto
        {
            public int Id { get; set; }
            public int VehicleId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        public class MonthlyPassCreateDto
        {
            [Required]
            public int VehicleId { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }
        }
    }
}
