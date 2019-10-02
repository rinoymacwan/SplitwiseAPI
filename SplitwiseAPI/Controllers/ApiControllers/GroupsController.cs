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
    public class GroupsController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public GroupsController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public IEnumerable<Groups> GetGroups()
        {
            return _context.Groups;
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groups = await _context.Groups.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            
            if (groups == null)
            {
                return NotFound();
            }
            var members = await _context.GroupMemberMappings.Where(g => g.GroupId == id).Select(k => k.User).ToListAsync();
            GroupAndMembers GAM = new GroupAndMembers() { Group = groups, Members = members };
            _context.UserFriendMappings.Where(u => u.UserId == id).Select(x => x.Friend);

            return Ok(GAM);
        }

        // GET: api/Groups/ByUserId
        [HttpGet("ByUserId/{id}")]
        public async Task<IActionResult> GetGroupsByUserId([FromRoute] int id)
        {
            System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAAAAAA");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var groups = await _context.Groups.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);

            //if (groups == null)
            //{
            //    return NotFound();
            //}
            var groups = await _context.GroupMemberMappings.Where(g => g.MemberId == id).Select(k => k.Group).ToListAsync();
            //GroupAndMembers GAM = new GroupAndMembers() { Group = groups, Members = members };
            //_context.UserFriendMappings.Where(u => u.UserId == id).Select(x => x.Friend);

            return Ok(groups);
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroups([FromRoute] int id, [FromBody] Groups groups)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groups.Id)
            {
                return BadRequest();
            }

            _context.Entry(groups).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsExists(id))
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

        // POST: api/Groups
        [HttpPost]
        public async Task<IActionResult> PostGroups([FromBody] Groups groups)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Groups.Add(groups);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroups", new { id = groups.Id }, groups);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();

            return Ok(groups);
        }

        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}