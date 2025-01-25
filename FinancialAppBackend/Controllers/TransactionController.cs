using Microsoft.AspNetCore.Mvc;
using FinancialAppBackend.Models;
using FinancialAppBackend.Servieces;

namespace FinancialAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _financialTransactionService;

        public TransactionController(TransactionService financialTransactionService)
        {
            _financialTransactionService = financialTransactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetFinancialTransactions()
        {
            return await _financialTransactionService.GetFinancialTransactionsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetFinancialTransaction(int id)
        {
            var financialTransaction = await _financialTransactionService.GetFinancialTransactionByIdAsync(id);

            if (financialTransaction == null)
            {
                return NotFound();
            }

            return financialTransaction;
        }

        [HttpPost]
        public async Task<ActionResult> CreateFinancialTransaction([FromBody] Common.Models.TransactionDto financialTransactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var financialTransaction = new Transaction
            {
                Amount = financialTransactionDto.Amount,
                Date = financialTransactionDto.TransactionDate,
                Comment = financialTransactionDto.Comment,
                TransactionTypeId = financialTransactionDto.TransactionTypeId
            };

            await _financialTransactionService.AddFinancialTransactionAsync(financialTransaction);

            return CreatedAtAction(nameof(GetFinancialTransaction), new { id = financialTransaction.Id }, financialTransaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFinancialTransaction([FromRoute] int id, [FromBody] Transaction financialTransaction)
        {
            if (id != financialTransaction.Id)
            {
                return BadRequest();
            }

            await _financialTransactionService.UpdateFinancialTransactionAsync(financialTransaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialTransaction(int id)
        {
            var financialTransaction = await _financialTransactionService.GetFinancialTransactionByIdAsync(id);

            if (financialTransaction == null)
            {
                return NotFound();
            }

            await _financialTransactionService.RemoveFinancialTransactionAsync(financialTransaction);
            return NoContent();
        }
    }
}