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
    public class PayersController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public PayersController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/Payers
        [HttpGet]
        public IEnumerable<Payers> GetPayers()
        {
            return _context.Payers;
        }

        // GET: api/Payers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = await _context.Payers.FindAsync(id);

            if (payers == null)
            {
                return NotFound();
            }

            return Ok(payers);
        }

        // GET: api/Payers/ByExpenseId/id
        [HttpGet("ByExpenseId/{id}")]
        public async Task<IActionResult> GetPayersByExpenseId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = await _context.Payers.Where(e => e.ExpenseId == id).ToListAsync();

            if (payers == null)
            {
                return NotFound();
            }

            return Ok(payers);
        }
        // PUT: api/Payers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayers([FromRoute] int id, [FromBody] Payers payers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payers.Id)
            {
                return BadRequest();
            }

            _context.Entry(payers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayersExists(id))
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

        // POST: api/Payers
        [HttpPost]
        public async Task<IActionResult> PostPayers([FromBody] Payers payers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Payers.Add(payers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayers", new { id = payers.Id }, payers);
        }

        // DELETE: api/Payers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = await _context.Payers.FindAsync(id);
            if (payers == null)
            {
                return NotFound();
            }

            _context.Payers.Remove(payers);
            await _context.SaveChangesAsync();

            return Ok(payers);
        }

        private bool PayersExists(int id)
        {
            return _context.Payers.Any(e => e.Id == id);
        }
    }
}