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

        //public UsersController()
        //{
        //    System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAA");
        //    this._usersRepository = new UsersRepository(new SplitwiseAPIContext());
        //}

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
        // POST: api/Users
        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] Users users)
        //{
        //    var x = await _usersRepository.Authenticate(users); 
        //    if(x.Email != null)
        //    {
        //        return Ok(x);
        //    } else
        //    {
        //        return NotFound();
        //    }

        //}
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]Users credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //System.Diagnostics.Debug.Write(credentials.UserName);
           // System.Diagnostics.Debug.Write(credentials.Password);
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            var user = await _userManager.FindByEmailAsync(credentials.UserName);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(user, identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            var x = await _userManager.Users.ToListAsync();
            //System.Diagnostics.Debug.Write(userName);
            //System.Diagnostics.Debug.Write(password);
            //System.Diagnostics.Debug.Write("BBBB\n");
            //System.Diagnostics.Debug.Write(x[0].UserName);
            //System.Diagnostics.Debug.Write(x[0].Password);
            //System.Diagnostics.Debug.Write(userToVerify.Password);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}