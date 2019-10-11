using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Repository.ExpensesRepository
{
    public class ExpensesRepository : IExpensesRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public ExpensesRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool ExpenseExists(int id)
        {
            return context.Expenses.Any(e => e.Id == id);
        }

        public void CreateExpense(Expenses Expense)
        {
            context.Expenses.Add(Expense);
        }

        public async Task DeleteExpense(Expenses Expense)
        {
            context.Expenses.Remove(Expense);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Expenses> GetExpenses()
        {
            return context.Expenses;
        }

        public Task<Expenses> GetExpense(int id)
        {
            return context.Expenses.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateExpense(Expenses Expense)
        {
            context.Entry(Expense).State = EntityState.Modified;
        }
    }
}
