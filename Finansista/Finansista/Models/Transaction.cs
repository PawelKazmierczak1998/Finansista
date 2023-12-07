using Finansista.Data.Enum;

namespace Finansista.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal amount { get; set; }
        public string? description { get; set; }
        public int balanceId { get; set; }
        public virtual Balance? Balance { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime? CreatedAt { get { return DateTime.Now; } }
    }
}
