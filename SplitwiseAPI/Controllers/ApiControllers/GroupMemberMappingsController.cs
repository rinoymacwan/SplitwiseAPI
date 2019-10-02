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
    public class GroupMemberMappingsController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public GroupMemberMappingsController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/GroupMemberMappings
        [HttpGet]
        public IEnumerable<GroupMemberMappings> GetGroupMemberMappings()
        {
            return _context.GroupMemberMappings;
        }
        // GET: api/GroupMemberMappings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupMemberMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupMemberMappings = await _context.GroupMemberMappings.FindAsync(id);

            if (groupMemberMappings == null)
            {
                return NotFound();
            }

            return Ok(groupMemberMappings);
        }

        // PUT: api/GroupMemberMappings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupMemberMappings([FromRoute] int id, [FromBody] GroupMemberMappings groupMemberMappings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupMemberMappings.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupMemberMappings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupMemberMappingsExists(id))
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

        // POST: api/GroupMemberMappings
        [HttpPost]
        public async Task<IActionResult> PostGroupMemberMappings([FromBody] GroupMemberMappings groupMemberMappings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GroupMemberMappings.Add(groupMemberMappings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupMemberMappings", new { id = groupMemberMappings.Id }, groupMemberMappings);
        }

        // DELETE: api/GroupMemberMappings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupMemberMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupMemberMappings = await _context.GroupMemberMappings.FindAsync(id);
            if (groupMemberMappings == null)
            {
                return NotFound();
            }

            _context.GroupMemberMappings.Remove(groupMemberMappings);
            await _context.SaveChangesAsync();

            return Ok(groupMemberMappings);
        }

        private bool GroupMemberMappingsExists(int id)
        {
            return _context.GroupMemberMappings.Any(e => e.Id == id);
        }
    }
}