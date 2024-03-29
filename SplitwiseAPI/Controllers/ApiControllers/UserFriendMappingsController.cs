﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UserFriendMappingsRepository;
using SplitwiseAPI.Repository.UsersRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserFriendMappingsController : ControllerBase
    {
        private readonly IUserFriendMappingsRepository _userFriendMappingsRepository;
        private readonly IUsersRepository _usersRepository;

        public UserFriendMappingsController(IUserFriendMappingsRepository userFriendMappingsRepository, IUsersRepository usersRepository)
        {
            this._userFriendMappingsRepository = userFriendMappingsRepository;
            this._usersRepository = usersRepository;
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
        public async Task<IActionResult> PutUserFriendMappings([FromRoute] string id, [FromBody] UserFriendMappings userFriendMappings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (int.Parse(id) != userFriendMappings.Id)
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
            _userFriendMappingsRepository.CreateUserFriendMapping(userFriendMappings);
            await _userFriendMappingsRepository.SaveAsync();

            return CreatedAtAction("GetUserFriendMappings", new { id = userFriendMappings.Id }, userFriendMappings);
        }
        [HttpPost("ByEmail/{id}")]
        public async Task<IActionResult> PostUserFriendMappingsByEmail([FromRoute] string id, [FromBody] Users user)
        {
            var x = await _userFriendMappingsRepository.CreateUserFriendMappingByEmail(id, user);
            return Ok(x);
        }

        // DELETE: api/UserFriendMappings/5
        [HttpDelete("delete/{user1?}/{user2?}")]
        public async Task<IActionResult> DeleteUserFriendMappings([FromQuery] string user1, [FromQuery] string user2)
        {
            if (!ModelState.IsValid)
            {   
                return BadRequest(ModelState);
            }

            await _userFriendMappingsRepository.DeleteUserFriendMappingByIds(user1, user2);

            await _userFriendMappingsRepository.SaveAsync();

            return Ok();
        }

        private bool UserFriendMappingsExists(string id)
        {
            return _userFriendMappingsRepository.UserFriendMappingExists(id);
        }
    }
}