using Common.Models;
using Common.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinancialAppBackend.Servieces
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DailyReportViewModel> GetDailyReportAsync(DateTime date)
        {
            var transactions = await _context.FinancialTransactions
                .Where(t => t.Date.Date == date.Date)
                .Include(t => t.TransactionType)
                .ToListAsync();

            var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);

            return new DailyReportViewModel
            {
                Date = date,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = ConvertToTransactionDto(transactions)
            };
        }

        public async Task<PeriodReportViewModel> GetPeriodReportAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.FinancialTransactions
                .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date)
                .Include(t => t.TransactionType)
                .ToListAsync();

            var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);

            return new PeriodReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Transactions = ConvertToTransactionDto(transactions)
            };
        }
        private List<TransactionDto> ConvertToTransactionDto(List<FinancialAppBackend.Models.Transaction> transactions)
        {
            return transactions.Select(t => new TransactionDto
            {
                Amount = t.Amount,
                Comment = t.Comment ?? string.Empty,
                TransactionDate = t.Date
            }).ToList();
        }
    }
}
