using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.PayeesRepository
{
    public class PayeesRepository : IPayeesRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public PayeesRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool PayeeExists(int id)
        {
            return context.Payees.Any(e => e.Id == id);
        }

        public void CreatePayee(Payees Payee)
        {
            context.Payees.Add(Payee);
        }

        public async Task DeletePayee(Payees Payee)
        {
            context.Payees.Remove(Payee);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public IEnumerable<Payees> GetPayees()
        {
            return context.Payees.Include(t => t.User);
        }
        public IEnumerable<Payees> GetPayeesByExpenseId(int id)
        {
            return context.Payees.Include(t => t.User).Where(e => e.ExpenseId == id).ToList();
        }

        public IEnumerable<Payees> GetPayeesByPayeeId(string id)
        {
            return this.GetPayees().Where(e => e.PayeeId == id).ToList();
        }

        public Task<Payees> GetPayee(int id)
        {
            return context.Payees.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdatePayee(Payees Payee)
        {
            context.Entry(Payee).State = EntityState.Modified;
        }
    }
}
