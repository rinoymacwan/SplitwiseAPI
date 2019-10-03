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
    public class ActivitiesController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public ActivitiesController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public IEnumerable<Activities> GetActivities()
        {
            return _context.Activities;
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activities = await _context.Activities.FindAsync(id);

            if (activities == null)
            {
                return NotFound();
            }

            return Ok(activities);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivities([FromRoute] int id, [FromBody] Activities activities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activities.Id)
            {
                return BadRequest();
            }

            _context.Entry(activities).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivitiesExists(id))
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

        // POST: api/Activities
        [HttpPost]
        public async Task<IActionResult> PostActivities([FromBody] Activities activities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Activities.Add(activities);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivities", new { id = activities.Id }, activities);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activities = await _context.Activities.FindAsync(id);
            if (activities == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activities);
            await _context.SaveChangesAsync();

            return Ok(activities);
        }

        private bool ActivitiesExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}