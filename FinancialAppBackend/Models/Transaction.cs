namespace FinancialAppBackend.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; }
        public string? Comment { get; set; }

        public TransactionType? TransactionType { get; set; } 
    }
}
