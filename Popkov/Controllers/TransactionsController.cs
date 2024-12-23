using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopkovFinance.Data;
using PopkovFinance.Models;

namespace PopkovFinance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction cannot be null");

            try
            {
                transaction.Validate();
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _context.Transactions.Include(t => t.Category).Include(t => t.User).ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _context.Transactions.Include(t => t.Category).Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUser(int userId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            var report = await _context.Transactions
                .GroupBy(t => t.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalIncome = g.Where(t => t.IsIncome).Sum(t => t.Amount),
                    TotalExpense = g.Where(t => !t.IsIncome).Sum(t => t.Amount),
                    NetBalance = g.Where(t => t.IsIncome).Sum(t => t.Amount) - g.Where(t => !t.IsIncome).Sum(t => t.Amount)
                })
                .ToListAsync();

            return Ok(report);
        }
    }
}
