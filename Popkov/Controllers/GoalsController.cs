using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopkovFinance.Data;
using PopkovFinance.Models;

namespace PopkovFinance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GoalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] Goal goal)
        {
            if (goal == null)
                return BadRequest("Goal cannot be null");

            try
            {
                goal.Validate();
                _context.Goals.Add(goal);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGoalById), new { id = goal.Id }, goal);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGoals()
        {
            var goals = await _context.Goals.ToListAsync();
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGoalById(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] Goal updatedGoal)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
                return NotFound();

            goal.Name = updatedGoal.Name;
            goal.TargetAmount = updatedGoal.TargetAmount;
            goal.CurrentAmount = updatedGoal.CurrentAmount;
            goal.DueDate = updatedGoal.DueDate;

            try
            {
                goal.Validate();
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
