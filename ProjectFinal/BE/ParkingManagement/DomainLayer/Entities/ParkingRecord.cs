using DomainLayer.Common.BaseEntities;

namespace DomainLayer.Entities
{
    public class ParkingRecord : AuditableEntity<int>
    {
        public int VehicleId { get;  set; }
        public int? ParkingSlotId { get;  set; }

        public DateTime TimeIn { get;  set; }
        public DateTime? TimeOut { get;  set; }
        public decimal? Fee { get;  set; }
        public string Note { get;  set; } = null!;
        public bool IsPaid { get;  set; } = false; 
        public Vehicle Vehicle { get; private set; } = null!;
        public ParkingSlot? ParkingSlot { get; private set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; private set; } = new List<PaymentTransaction>();

        public void CheckOut(decimal fee)
        {
            TimeOut = DateTime.Now;
            Fee = fee;
            IsPaid = false; 
        }

        public void MarkPaid()
        {
            IsPaid = true;
        }

    }
}
