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
using SplitwiseAPI.Repository.GroupMemberMappingsRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMemberMappingsController : ControllerBase
    {
        private readonly IGroupMemberMappingsRepository _groupMemberMappingsRepository;

        public GroupMemberMappingsController(IGroupMemberMappingsRepository groupMemberMappingsRepository)
        {
            this._groupMemberMappingsRepository = groupMemberMappingsRepository;
        }

        // GET: api/GroupMemberMappings
        [HttpGet]
        public IEnumerable<GroupMemberMappings> GetGroupMemberMappings()
        {
            return _groupMemberMappingsRepository.GetGroupMemberMappings();
        }
        // GET: api/GroupMemberMappings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupMemberMappings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupMemberMappings = await _groupMemberMappingsRepository.GetGroupMemberMapping(id);

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

            _groupMemberMappingsRepository.UpdateGroupMemberMapping(groupMemberMappings);

            try
            {
                await _groupMemberMappingsRepository.Save();
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

            await _groupMemberMappingsRepository.CreateGroupMemberMapping(groupMemberMappings);
            await _groupMemberMappingsRepository.Save();

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

            var groupMemberMappings = await _groupMemberMappingsRepository.GetGroupMemberMapping(id);
            if (groupMemberMappings == null)
            {
                return NotFound();
            }

            await _groupMemberMappingsRepository.DeleteGroupMemberMapping(groupMemberMappings);
            await _groupMemberMappingsRepository.Save();

            return Ok(groupMemberMappings);
        }

        private bool GroupMemberMappingsExists(int id)
        {
            return _groupMemberMappingsRepository.GroupMemberMappingExists(id);
        }
    }
}