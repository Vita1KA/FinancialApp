using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;
using FinancialAppBackend.Servieces;

namespace FinancialAppBackend.Tests
{
    public class FinancialServiceTests
    {
        private static readonly TransactionType IncomeType = new TransactionType { Name = "Income" };
        private static readonly TransactionType ExpenseType = new TransactionType { Name = "Expense" };

        private AppDbContext CreateInMemoryDbContext(IEnumerable<Transaction> transactions)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            var context = new AppDbContext(options);
            context.FinancialTransactions.AddRange(transactions);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetDailyReportAsync_ReturnsCorrectReport()
        {
            // Arrange
            var date = new DateTime(2024, 12, 21);
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Date = date, Amount = 100, TransactionType = IncomeType },
                new Transaction { Id = 2, Date = date, Amount = -50, TransactionType = ExpenseType }
            };

            using var context = CreateInMemoryDbContext(transactions);
            var service = new ReportService(context);

            // Act
            var report = await service.GetDailyReportAsync(date);

            // Assert
            Assert.Equal(date, report.Date);
            Assert.Equal(100, report.TotalIncome);
            Assert.Equal(-50, report.TotalExpenses);
            Assert.Equal(2, report.Transactions.Count);
        }

        [Fact]
        public async Task GetPeriodReportAsync_ReturnsCorrectReport()
        {
            // Arrange
            var startDate = new DateTime(2024, 12, 20);
            var endDate = new DateTime(2024, 12, 21);

            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Date = new DateTime(2024, 12, 20), Amount = 200, TransactionType = IncomeType },
                new Transaction { Id = 2, Date = new DateTime(2024, 12, 21), Amount = -80, TransactionType = ExpenseType }
            };

            using var context = CreateInMemoryDbContext(transactions);
            var service = new ReportService(context);

            // Act
            var report = await service.GetPeriodReportAsync(startDate, endDate);

            // Assert
            Assert.Equal(startDate, report.StartDate);
            Assert.Equal(endDate, report.EndDate);
            Assert.Equal(200, report.TotalIncome);
            Assert.Equal(-80, report.TotalExpenses);
            Assert.Equal(2, report.Transactions.Count);
        }

        [Fact]
        public async Task GetDailyReportAsync_ReturnsEmptyReportForNoTransactions()
        {
            // Arrange
            var date = new DateTime(2024, 12, 21);
            using var context = CreateInMemoryDbContext(new List<Transaction>());
            var service = new ReportService(context);

            // Act
            var report = await service.GetDailyReportAsync(date);

            // Assert
            Assert.Equal(date, report.Date);
            Assert.Equal(0, report.TotalIncome);
            Assert.Equal(0, report.TotalExpenses);
            Assert.Empty(report.Transactions);
        }

        [Fact]
        public async Task GetPeriodReportAsync_ReturnsEmptyReportForNoTransactions()
        {
            // Arrange
            var startDate = new DateTime(2024, 12, 20);
            var endDate = new DateTime(2024, 12, 21);

            using var context = CreateInMemoryDbContext(new List<Transaction>());
            var service = new ReportService(context);

            // Act
            var report = await service.GetPeriodReportAsync(startDate, endDate);

            // Assert
            Assert.Equal(startDate, report.StartDate);
            Assert.Equal(endDate, report.EndDate);
            Assert.Equal(0, report.TotalIncome);
            Assert.Equal(0, report.TotalExpenses);
            Assert.Empty(report.Transactions);
        }
    }
}
