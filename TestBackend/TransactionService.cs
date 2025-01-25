using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;
using FinancialAppBackend.Servieces;
using Xunit;

namespace FinancialAppBackend.Tests
{
    public class TransactionServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetFinancialTransactionsAsync_ReturnsAllTransactions()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionService(context);

            var transactionType = new TransactionType { Name = "Income" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            var transaction1 = new Transaction { Amount = 100, TransactionTypeId = transactionType.Id };
            var transaction2 = new Transaction { Amount = 200, TransactionTypeId = transactionType.Id };
            context.FinancialTransactions.AddRange(transaction1, transaction2);
            await context.SaveChangesAsync();

            // Act
            var transactions = await service.GetFinancialTransactionsAsync();

            // Assert
            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        public async Task GetFinancialTransactionByIdAsync_ReturnsCorrectTransaction()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionService(context);

            var transactionType = new TransactionType { Name = "Income" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            var transaction = new Transaction { Amount = 150, TransactionTypeId = transactionType.Id };
            context.FinancialTransactions.Add(transaction);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetFinancialTransactionByIdAsync(transaction.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transaction.Amount, result?.Amount);
        }

        [Fact]
        public async Task AddFinancialTransactionAsync_AddsNewTransaction()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionService(context);

            var transactionType = new TransactionType { Name = "Income" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            var transaction = new Transaction { Amount = 200, TransactionTypeId = transactionType.Id };

            // Act
            await service.AddFinancialTransactionAsync(transaction);

            // Assert
            var result = await context.FinancialTransactions.FindAsync(transaction.Id);
            Assert.NotNull(result);
            Assert.Equal(transaction.Amount, result?.Amount);
        }

        [Fact]
        public async Task UpdateFinancialTransactionAsync_UpdatesTransaction()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionService(context);

            var transactionType = new TransactionType { Name = "Income" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            var transaction = new Transaction { Amount = 300, TransactionTypeId = transactionType.Id };
            context.FinancialTransactions.Add(transaction);
            await context.SaveChangesAsync();

            transaction.Amount = 500;

            // Act
            await service.UpdateFinancialTransactionAsync(transaction);

            // Assert
            var result = await context.FinancialTransactions.FindAsync(transaction.Id);
            Assert.NotNull(result);
            Assert.Equal(500, result?.Amount);
        }

        [Fact]
        public async Task RemoveFinancialTransactionAsync_RemovesTransaction()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionService(context);

            var transactionType = new TransactionType { Name = "Income" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            var transaction = new Transaction { Amount = 100, TransactionTypeId = transactionType.Id };
            context.FinancialTransactions.Add(transaction);
            await context.SaveChangesAsync();

            // Act
            await service.RemoveFinancialTransactionAsync(transaction);

            // Assert
            var result = await context.FinancialTransactions.FindAsync(transaction.Id);
            Assert.Null(result);
        }
    }
}
