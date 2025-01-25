using Microsoft.AspNetCore.Mvc;
using FinancialAppBackend.Models;
using FinancialAppBackend.Servieces;

namespace FinancialAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly TransactionTypeService _transactionTypeService;

        public TransactionTypeController(TransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypes()
        {
            return await _transactionTypeService.GetTransactionTypesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionType>> GetTransactionType(int id)
        {
            var transactionType = await _transactionTypeService.GetTransactionTypeByIdAsync(id);

            if (transactionType == null)
            {
                return NotFound();
            }

            return transactionType;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionType>> CreateTransactionType(TransactionType transactionType)
        {
            await _transactionTypeService.AddTransactionTypeAsync(transactionType);
            return CreatedAtAction(nameof(GetTransactionType), new { id = transactionType.Id }, transactionType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionType(int id, TransactionType transactionType)
        {
            if (id != transactionType.Id)
            {
                return BadRequest();
            }

            await _transactionTypeService.UpdateTransactionTypeAsync(transactionType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionType(int id)
        {
            var transactionType = await _transactionTypeService.GetTransactionTypeByIdAsync(id);

            if (transactionType == null)
            {
                return NotFound();
            }

            await _transactionTypeService.RemoveTransactionTypeAsync(transactionType);
            return NoContent();
        }
    }
}
