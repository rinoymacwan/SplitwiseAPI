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
    public class UserFriendMappingsController : ControllerBase
    {
        private readonly SplitwiseAPIContext _context;

        public UserFriendMappingsController(SplitwiseAPIContext context)
        {
            _context = context;
        }

        // GET: api/UserFriendMappings
        [HttpGet]
        public IEnumerable<UserFriendMappings> GetUserFriendMappings()
        {
            return _context.UserFriendMappings;
        }

        // GET: api/UserFriendMappings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserFriendMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFriendMappings = await _context.UserFriendMappings.FindAsync(id);

            if (userFriendMappings == null)
            {
                return NotFound();
            }

            return Ok(userFriendMappings);
        }

        // PUT: api/UserFriendMappings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFriendMappings([FromRoute] int id, [FromBody] UserFriendMappings userFriendMappings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userFriendMappings.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFriendMappings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFriendMappingsExists(id))
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

        // POST: api/UserFriendMappings
        [HttpPost]
        public async Task<IActionResult> PostUserFriendMappings([FromBody] UserFriendMappings userFriendMappings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserFriendMappings otherEntry = new UserFriendMappings() { UserId = userFriendMappings.FriendId, FriendId = userFriendMappings.UserId };
            _context.UserFriendMappings.Add(userFriendMappings);
            _context.UserFriendMappings.Add(otherEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFriendMappings", new { id = userFriendMappings.Id }, userFriendMappings);
        }

        // DELETE: api/UserFriendMappings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFriendMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFriendMappings = await _context.UserFriendMappings.FindAsync(id);
            if (userFriendMappings == null)
            {
                return NotFound();
            }

            var x =await _context.UserFriendMappings.Where(k => k.UserId == userFriendMappings.FriendId && k.FriendId == userFriendMappings.UserId).FirstOrDefaultAsync();
            _context.UserFriendMappings.Remove(userFriendMappings);
            _context.UserFriendMappings.Remove(x);

            await _context.SaveChangesAsync();

            return Ok(userFriendMappings);
        }

        private bool UserFriendMappingsExists(int id)
        {
            return _context.UserFriendMappings.Any(e => e.Id == id);
        }
    }
}