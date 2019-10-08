using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UserFriendMappingsRepository;
using SplitwiseAPI.Repository.UsersRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFriendMappingsController : ControllerBase
    {
        private readonly IUserFriendMappingsRepository _userFriendMappingsRepository;

        public UserFriendMappingsController(IUserFriendMappingsRepository userFriendMappingsRepository)
        {
            this._userFriendMappingsRepository = userFriendMappingsRepository;
        }

        // GET: api/UserFriendMappings
        [HttpGet]
        public IEnumerable<UserFriendMappings> GetUserFriendMappings()
        {
            return _userFriendMappingsRepository.GetUserFriendMappings();
        }

        // GET: api/UserFriendMappings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserFriendMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFriendMappings = await _userFriendMappingsRepository.GetUserFriendMapping(id);

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

            _userFriendMappingsRepository.UpdateUserFriendMapping(userFriendMappings);

            try
            {
                await _userFriendMappingsRepository.SaveAsync();
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
            _userFriendMappingsRepository.CreateUserFriendMapping(userFriendMappings);
            _userFriendMappingsRepository.CreateUserFriendMapping(otherEntry);
            await _userFriendMappingsRepository.SaveAsync();

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

            var userFriendMappings = await _userFriendMappingsRepository.GetUserFriendMapping(id);
            if (userFriendMappings == null)
            {
                return NotFound();
            }

            await _userFriendMappingsRepository.DeleteUserFriendMapping(userFriendMappings);

            await _userFriendMappingsRepository.SaveAsync();

            return Ok(userFriendMappings);
        }

        private bool UserFriendMappingsExists(int id)
        {
            return _userFriendMappingsRepository.UserFriendMappingExists(id);
        }
    }
}