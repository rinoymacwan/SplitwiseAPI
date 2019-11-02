using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.PayeesRepository;

namespace SplitwiseAPI.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayeesController : ControllerBase
    {
        private readonly IPayeesRepository _payeesRepository;

        public PayeesController(IPayeesRepository payeesRepository)
        {
            this._payeesRepository = payeesRepository;
        }

        // GET: api/Payees
        [HttpGet]
        public IEnumerable<Payees> GetPayees()
        {
            return _payeesRepository.GetPayees();
        }

        // GET: api/Payees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = await _payeesRepository.GetPayee(id);

            if (payees == null)
            {
                return NotFound();
            }

            return Ok(payees);
        }

        // GET: api/Payees/ByExpenseId/id
        [HttpGet("ByExpenseId/{id}")]
        public async Task<IActionResult> GetPayeesByExpenseId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = _payeesRepository.GetPayees().Where(e => e.ExpenseId == id).ToList();

            if (payees == null)
            {
                return NotFound();
            }

            return Ok(payees);
        }

        // GET: api/Payees/ByExpenseId/id
        [HttpGet("ByPayeeId/{id}")]
        public async Task<IActionResult> GetPayeesByPayeeId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = _payeesRepository.GetPayees().Where(e => e.PayeeId == id).ToList();

            if (payees == null)
            {
                return NotFound();
            }

            return Ok(payees);
        }

        // PUT: api/Payees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayees([FromRoute] int id, [FromBody] Payees payees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payees.Id)
            {
                return BadRequest();
            }

            _payeesRepository.UpdatePayee(payees);

            try
            {
                await _payeesRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayeesExists(id))
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

        // POST: api/Payees
        [HttpPost]
        public async Task<IActionResult> PostPayees([FromBody] Payees payees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _payeesRepository.CreatePayee(payees);
            await _payeesRepository.Save();

            return CreatedAtAction("GetPayees", new { id = payees.Id }, payees);
        }

        // DELETE: api/Payees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payees = await _payeesRepository.GetPayee(id);
            if (payees == null)
            {
                return NotFound();
            }

            await _payeesRepository.DeletePayee(payees);
            await _payeesRepository.Save();

            return Ok(payees);
        }

        private bool PayeesExists(int id)
        {
            return _payeesRepository.PayeeExists(id);
        }
    }
}