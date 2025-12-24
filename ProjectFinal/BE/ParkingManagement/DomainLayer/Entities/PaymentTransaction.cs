using DomainLayer.Common.BaseEntities;
using DomainLayer.Enums;

namespace DomainLayer.Entities
{
    public class PaymentTransaction : AuditableEntity<int>
    {
        public int ParkingRecordId { get;  set; }
        public PaymentMethod PaymentMethod { get;  set; }
        public string? Provider { get;  set; }
        public string? TransactionCode { get;  set; }
        public decimal Amount { get;  set; }

        public PaymentStatus Status { get;  set; }
        public DateTime? PaymentDate { get;  set; }

        public ParkingRecord ParkingRecord { get;  set; } = null!;

        public static PaymentTransaction Create(
            int parkingRecordId,
            decimal amount,
            PaymentMethod method,
            string? provider = null)
        {
            var isCash = method == PaymentMethod.Cash;

            return new PaymentTransaction
            {
                ParkingRecordId = parkingRecordId,
                Amount = amount,
                PaymentMethod = method,
                Provider = provider,
                Status = isCash ? PaymentStatus.Success : PaymentStatus.Pending,
                PaymentDate = isCash ? DateTime.UtcNow : null
            };
        }

        public void UpdateStatus(PaymentStatus status, string? transactionCode = null)
        {
            Status = status;
            TransactionCode = transactionCode;

            if (status == PaymentStatus.Success)
            {
                PaymentDate = DateTime.UtcNow;
            }
        }
    }
}
