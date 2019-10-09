using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.SettlementsRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementsController : ControllerBase
    {
        private readonly ISettlementsRepository _settlementsRepository;

        public SettlementsController(ISettlementsRepository settlementsRepository)
        {
            this._settlementsRepository = settlementsRepository;
        }

        // GET: api/Settlements
        [HttpGet]
        public IEnumerable<Settlements> GetSettlements()
        {
            return _settlementsRepository.GetSettlements();
        }

        // GET: api/Settlements/GetByUserId
        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetSettlementsByUserId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = _settlementsRepository.GetSettlements().Where(s => s.PayeeId == id || s.PayerId == id).ToList();

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // GET: api/Settlements/GetByGroupId
        [HttpGet("GetByGroupId/{id}")]
        public async Task<IActionResult> GetSettlementsByGroupId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = _settlementsRepository.GetSettlements().Where(s => s.GroupId == id).ToList();

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // GET: api/Settlements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSettlements([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _settlementsRepository.GetSettlement(id);

            if (settlements == null)
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        // PUT: api/Settlements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettlements([FromRoute] int id, [FromBody] Settlements settlements)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settlements.Id)
            {
                return BadRequest();
            }

            _settlementsRepository.UpdateSettlement(settlements);

            try
            {
                await _settlementsRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettlementsExists(id))
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

        // POST: api/Settlements
        [HttpPost]
        public async Task<IActionResult> PostSettlements([FromBody] Settlements settlements)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _settlementsRepository.CreateSettlement(settlements);
            await _settlementsRepository.Save();

            return CreatedAtAction("GetSettlements", new { id = settlements.Id }, settlements);
        }

        // DELETE: api/Settlements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSettlements([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var settlements = await _settlementsRepository.GetSettlement(id);
            if (settlements == null)
            {
                return NotFound();
            }

            await _settlementsRepository.DeleteSettlement(settlements);
            await _settlementsRepository.Save();

            return Ok(settlements);
        }

        private bool SettlementsExists(int id)
        {
            return _settlementsRepository.SettlementExists(id);
        }
    }
}