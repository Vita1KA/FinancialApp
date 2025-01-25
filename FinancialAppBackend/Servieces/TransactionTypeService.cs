using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;

namespace FinancialAppBackend.Servieces
{
    public class TransactionTypeService
    {
        private readonly AppDbContext _context;

        public TransactionTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionType>> GetTransactionTypesAsync()
        {
            return await _context.TransactionTypes.ToListAsync();
        }

        public async Task<TransactionType?> GetTransactionTypeByIdAsync(int id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }

        public async Task AddTransactionTypeAsync(TransactionType transactionType)
        {
            _context.TransactionTypes.Add(transactionType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionTypeAsync(TransactionType transactionType)
        {
            _context.Entry(transactionType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTransactionTypeAsync(TransactionType transactionType)
        {
            _context.TransactionTypes.Remove(transactionType);
            await _context.SaveChangesAsync();
        }
    }
}
