using System;
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
using SplitwiseAPI.Repository.ExpensesRepository;
using SplitwiseAPI.Repository.GroupMemberMappingsRepository;
using SplitwiseAPI.Repository.GroupsRepository;
using SplitwiseAPI.Repository.UserFriendMappingsRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly IGroupMemberMappingsRepository _groupMemberMappingsRepository;
        private readonly IUserFriendMappingsRepository _userFriendMappingsRepository;
        private readonly IExpensesRepository _expensesRepository;
        public GroupsController(IGroupsRepository groupsRepository, IGroupMemberMappingsRepository groupMemberMappingsRepository, IUserFriendMappingsRepository userFriendMappingsRepository, IExpensesRepository expensesRepository)
        {
            this._groupsRepository = groupsRepository;
            this._groupMemberMappingsRepository = groupMemberMappingsRepository;
            this._userFriendMappingsRepository = userFriendMappingsRepository;
            this._expensesRepository = expensesRepository;
        }

        // GET: api/Groups
        [HttpGet]
        public IEnumerable<Groups> GetGroups()
        {
            return _groupsRepository.GetGroups();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupDetails = await _groupsRepository.GetGroupWithDetails(id);
            
            if (groupDetails == null)
            {
                return NotFound();
            }

            return Ok(groupDetails);
        }

        // GET: api/Groups/ByUserId
        [HttpGet("ByUserId/{id}")]
        public async Task<IActionResult> GetGroupsByUserId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groups = _groupsRepository.GetGroupsByUserId(id);

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

            _groupsRepository.UpdateGroup(groups);

            try
            {
                await _groupsRepository.Save();
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

            _groupsRepository.CreateGroup(groups);
            await _groupsRepository.Save();

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

            var groups = await _groupsRepository.GetGroup(id);
            if (groups == null)
            {
                return NotFound();
            }
            
            await _groupsRepository.DeleteGroup(groups);
            await _groupsRepository.Save();

            return Ok(groups);
        }

        private bool GroupsExists(int id)
        {
            return _groupsRepository.GroupExists(id);
        }
    }
}