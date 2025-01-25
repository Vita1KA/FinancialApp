using Common.Models;

namespace Common.ViewModels
{
    public class DailyReportViewModel
    {
        public DateTime Date { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new(); 
    }
}
