using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SplitwiseAPI.DomainModel;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UsersRepository;
using SplitwiseAPI.Utility;
using SplitwiseAPI.Utility.Helpers;

namespace SplitwiseAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<Users> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public UsersController(IUsersRepository usersRepository, UserManager<Users> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this._usersRepository = usersRepository;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

        }

        // GET: api/Users
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "ApiUser")]
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            
            return _usersRepository.GetUsers();
        }

        // GET: api/Users/Friends
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("{id}/friends")]
        public IEnumerable<Users> GetAllFriends([FromRoute] string id)
        {
            return _usersRepository.GetAllFriends(id);
        }

        // GET: api/Users/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _usersRepository.GetUser(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Users/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "ApiUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] string id, [FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.Id)
            {
                return BadRequest();
            }

            _usersRepository.UpdateUser(users);

            try
            {
                await _usersRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _usersRepository.CreateUser(users);
            await _usersRepository.Save();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "ApiUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _usersRepository.GetUser(id);
            if (users == null)
            {
                return NotFound();
            }
            await _usersRepository.DeleteUser(users);
            await _usersRepository.Save();

            return Ok(users);
        }

        private bool UsersExists(string id)
        {
            return _usersRepository.UserExists(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]Users credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _usersRepository.Login(credentials);
            
            if (result == false)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }
            else
            {
                var jwt = await _usersRepository.GenerateJWT(credentials); 
                return new OkObjectResult(jwt);
            }
        }
    }
}