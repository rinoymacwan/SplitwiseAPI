using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.PayersRepository
{
    public class PayersRepository : IPayersRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public PayersRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool PayerExists(int id)
        {
            return context.Payers.Any(e => e.Id == id);
        }

        public void CreatePayer(Payers Payer)
        {
            context.Payers.Add(Payer);
        }

        public async Task DeletePayer(Payers Payer)
        {
            context.Payers.Remove(Payer);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public IEnumerable<Payers> GetPayers()
        {
            return context.Payers.Include(t => t.User);
        }

        public IEnumerable<Payers> GetPayersByExpenseId(int id)
        {
            return context.Payers.Include(t => t.User).Where(e => e.ExpenseId == id).ToList();
        }

        public IEnumerable<Payers> GetPayersByPayerId(string id)
        {
            return context.Payers.Include(t => t.User).Where(e => e.PayerId == id).ToList();
        }

        public Task<Payers> GetPayer(int id)
        {
            return context.Payers.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdatePayer(Payers Payer)
        {
            context.Entry(Payer).State = EntityState.Modified;
        }
    }
}
