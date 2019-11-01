using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UsersRepository;

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
        public UsersController(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            
            return _usersRepository.GetUsers();
        }

        // GET: api/Users/Friends
        [HttpGet("{id}/friends")]
        public IEnumerable<Users> GetAllFriends([FromRoute] int id)
        {
            return _usersRepository.GetAllFriends(id);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] Users users)
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

            _usersRepository.CreateUser(users);
            await _usersRepository.Save();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
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

        private bool UsersExists(int id)
        {
            return _usersRepository.UserExists(id);
        }
        // POST: api/Users
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Users users)
        {
            var x = await _usersRepository.Authenticate(users); 
            if(x.Email != null)
            {
                return Ok(x);
            } else
            {
                return NotFound();
            }
            
        }
    }
}