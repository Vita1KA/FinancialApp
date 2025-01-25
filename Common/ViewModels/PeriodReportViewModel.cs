using Common.Models;

namespace Common.ViewModels
{
    public class PeriodReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new(); 
    }
}
