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
    public class SettlementsController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public SettlementsController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/Settlements
        [HttpGet]
        public IEnumerable<Settlements> GetSettlements()
        {
            return _context.Settlements;
        }

        // GET: api/Settlements/GetByUserId
        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetSettlementsByUserId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _context.Settlements.Where(s => s.PayeeId == id || s.PayerId == id).ToListAsync();

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // GET: api/Settlements/GetByGroupId
        [HttpGet("GetByGroupId/{id}")]
        public async Task<IActionResult> GetSettlementsByGroupId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _context.Settlements.Where(s => s.GroupId == id).ToListAsync();

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // GET: api/Settlements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSettlements([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _context.Settlements.FindAsync(id);

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // PUT: api/Settlements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettlements([FromRoute] int id, [FromBody] Settlements settlements)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settlements.Id)
            {
                return BadRequest();
            }

            _context.Entry(settlements).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettlementsExists(id))
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

        // POST: api/Settlements
        [HttpPost]
        public async Task<IActionResult> PostSettlements([FromBody] Settlements settlements)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Settlements.Add(settlements);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSettlements", new { id = settlements.Id }, settlements);
        }

        // DELETE: api/Settlements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettlements([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _context.Settlements.FindAsync(id);
            if (settlements == null)
            {
                return NotFound();
            }

            _context.Settlements.Remove(settlements);
            await _context.SaveChangesAsync();

            return Ok(settlements);
        }

        private bool SettlementsExists(int id)
        {
            return _context.Settlements.Any(e => e.Id == id);
        }
    }
}