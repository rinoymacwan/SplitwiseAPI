using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.PayersRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayersController : ControllerBase
    {
        private readonly IPayersRepository _payersRepository;

        public PayersController(IPayersRepository payersRepository)
        {
            this._payersRepository = payersRepository;
        }

        // GET: api/Payers
        [HttpGet]
        public IEnumerable<Payers> GetPayers()
        {
            return _payersRepository.GetPayers();
        }

        // GET: api/Payers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = await _payersRepository.GetPayer(id);

            if (payers == null)
            {
                return NotFound();
            }

            return Ok(payers);
        }

        // GET: api/Payers/ByExpenseId/id
        [HttpGet("ByExpenseId/{id}")]
        public async Task<IActionResult> GetPayersByExpenseId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = _payersRepository.GetPayers().Where(e => e.ExpenseId == id).ToList();

            if (payers == null)
            {
                return NotFound();
            }

            return Ok(payers);
        }

        // GET: api/Payers/ByPayerId/id
        [HttpGet("ByPayerId/{id}")]
        public async Task<IActionResult> GetPayersByPayerId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = _payersRepository.GetPayers().Where(e => e.PayerId == id).ToList();

            if (payers == null)
            {
                return NotFound();
            }

            return Ok(payers);
        }

        // PUT: api/Payers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayers([FromRoute] int id, [FromBody] Payers payers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payers.Id)
            {
                return BadRequest();
            }

            _payersRepository.UpdatePayer(payers);

            try
            {
                _payersRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayersExists(id))
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

        // POST: api/Payers
        [HttpPost]
        public async Task<IActionResult> PostPayers([FromBody] Payers payers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _payersRepository.CreatePayer(payers);
            await _payersRepository.Save();

            return CreatedAtAction("GetPayers", new { id = payers.Id }, payers);
        }

        // DELETE: api/Payers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payers = await _payersRepository.GetPayer(id);
            if (payers == null)
            {
                return NotFound();
            }

            await _payersRepository.DeletePayer(payers);
            await _payersRepository.Save();

            return Ok(payers);
        }

        private bool PayersExists(int id)
        {
            return _payersRepository.PayerExists(id);
        }
    }
}