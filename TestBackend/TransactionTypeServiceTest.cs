using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;
using FinancialAppBackend.Servieces;


namespace FinancialAppBackend.Tests
{
    public class TransactionTypeServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetTransactionTypesAsync_ReturnsAllTypes()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionTypeService(context);

            context.TransactionTypes.AddRange(new TransactionType { Name = "Income" }, new TransactionType { Name = "Expense" });
            await context.SaveChangesAsync();

            // Act
            var transactionTypes = await service.GetTransactionTypesAsync();

            // Assert
            Assert.Equal(2, transactionTypes.Count);
        }

        [Fact]
        public async Task GetTransactionTypeByIdAsync_ReturnsCorrectType()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionTypeService(context);

            var transactionType = new TransactionType { Name = "Savings" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetTransactionTypeByIdAsync(transactionType.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionType.Name, result?.Name);
        }

        [Fact]
        public async Task AddTransactionTypeAsync_AddsNewType()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionTypeService(context);

            var transactionType = new TransactionType { Name = "Loan" };

            // Act
            await service.AddTransactionTypeAsync(transactionType);

            // Assert
            var result = await context.TransactionTypes.FindAsync(transactionType.Id);
            Assert.NotNull(result);
            Assert.Equal(transactionType.Name, result?.Name);
        }

        [Fact]
        public async Task UpdateTransactionTypeAsync_UpdatesType()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionTypeService(context);

            var transactionType = new TransactionType { Name = "Investment" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            transactionType.Name = "Updated Investment";

            // Act
            await service.UpdateTransactionTypeAsync(transactionType);

            // Assert
            var result = await context.TransactionTypes.FindAsync(transactionType.Id);
            Assert.NotNull(result);
            Assert.Equal("Updated Investment", result?.Name);
        }

        [Fact]
        public async Task RemoveTransactionTypeAsync_RemovesType()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var service = new TransactionTypeService(context);

            var transactionType = new TransactionType { Name = "Expense" };
            context.TransactionTypes.Add(transactionType);
            await context.SaveChangesAsync();

            // Act
            await service.RemoveTransactionTypeAsync(transactionType);

            // Assert
            var result = await context.TransactionTypes.FindAsync(transactionType.Id);
            Assert.Null(result);
        }
    }
}
