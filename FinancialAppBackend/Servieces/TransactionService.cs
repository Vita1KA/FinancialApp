using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;

namespace FinancialAppBackend.Servieces
{
    public class TransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetFinancialTransactionsAsync()
        {
            return await _context.FinancialTransactions.Include(t => t.TransactionType).ToListAsync();
        }

        public async Task<Transaction?> GetFinancialTransactionByIdAsync(int id)
        {
            return await _context.FinancialTransactions.Include(t => t.TransactionType).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddFinancialTransactionAsync(Transaction financialTransaction)
        {
            _context.FinancialTransactions.Add(financialTransaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFinancialTransactionAsync(Transaction financialTransaction)
        {
            var transactionToUpdate = await _context.FinancialTransactions.FindAsync(financialTransaction.Id);
            if (transactionToUpdate == null)
            {
                throw new Exception("Financial transaction not found");
            }

            _context.Entry(transactionToUpdate).CurrentValues.SetValues(financialTransaction);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFinancialTransactionAsync(Transaction financialTransaction)
        {
            _context.FinancialTransactions.Remove(financialTransaction);
            await _context.SaveChangesAsync();
        }
    }
}
