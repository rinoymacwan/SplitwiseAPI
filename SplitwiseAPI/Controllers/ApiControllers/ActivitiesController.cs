using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.ActivitiesRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesRepository _activitiesRepository;

        public ActivitiesController(IActivitiesRepository activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        // GET: api/Activities
        [HttpGet]
        public IEnumerable<Activities> GetActivities()
        {
            return _activitiesRepository.GetActivities();
        }
        [HttpGet("ByUserId/{id}")]
        public IEnumerable<Activities> GetActivitiesByUserId([FromRoute] int id)
        {
            //Thread.Sleep(TimeSpan.FromSeconds(5));
            return _activitiesRepository.GetActivities().Where(k =>k.UserId==id);
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activities = await _activitiesRepository.GetActivity(id);

            if (activities == null)
            {
                return NotFound();
            }

            return Ok(activities);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivities([FromRoute] int id, [FromBody] Activities activities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activities.Id)
            {
                return BadRequest();
            }

            _activitiesRepository.UpdateActivity(activities);

            try
            {
                await _activitiesRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivitiesExists(id))
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

        // POST: api/Activities
        [HttpPost]
        public async Task<IActionResult> PostActivities([FromBody] Activities activities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _activitiesRepository.CreateActivity(activities);
            await _activitiesRepository.Save();

            return CreatedAtAction("GetActivities", new { id = activities.Id }, activities);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activities = await _activitiesRepository.GetActivity(id);
            if (activities == null)
            {
                return NotFound();
            }

            await _activitiesRepository.DeleteActivity(activities);
            await _activitiesRepository.Save();

            return Ok(activities);
        }

        private bool ActivitiesExists(int id)
        {
            return _activitiesRepository.ActivityExists(id);
        }
    }
}