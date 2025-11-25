using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MonthlyPass
    {
        public int MonthlyPassId { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int UserId { get; set; }         // người sở hữu
        public User User { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal MonthlyPrice { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
