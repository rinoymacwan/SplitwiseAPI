using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayeesController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public PayeesController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/Payees
        [HttpGet]
        public IEnumerable<Payees> GetPayees()
        {
            return _context.Payees;
        }

        // GET: api/Payees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = await _context.Payees.FindAsync(id);

            if (payees == null)
            {
                return NotFound();
            }

            return Ok(payees);
        }

        // GET: api/Payees/ByExpenseId/id
        [HttpGet("ByExpenseId/{id}")]
        public async Task<IActionResult> GetPayeesByExpenseId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = await _context.Payees.Where(e => e.ExpenseId == id).ToListAsync();

            if (payees == null)
            {
                return NotFound();
            }

            return Ok(payees);
        }

        // PUT: api/Payees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayees([FromRoute] int id, [FromBody] Payees payees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payees.Id)
            {
                return BadRequest();
            }

            _context.Entry(payees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayeesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Payees
        [HttpPost]
        public async Task<IActionResult> PostPayees([FromBody] Payees payees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Payees.Add(payees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayees", new { id = payees.Id }, payees);
        }

        // DELETE: api/Payees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = await _context.Payees.FindAsync(id);
            if (payees == null)
            {
                return NotFound();
            }

            _context.Payees.Remove(payees);
            await _context.SaveChangesAsync();

            return Ok(payees);
        }

        private bool PayeesExists(int id)
        {
            return _context.Payees.Any(e => e.Id == id);
        }
    }
}