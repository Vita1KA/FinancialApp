namespace Common.Models
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionTypeId { get; set; }
    }
}

