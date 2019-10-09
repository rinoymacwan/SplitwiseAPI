using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.ExpensesRepository
{
    public interface IExpensesRepository
    {
        IEnumerable<Expenses> GetExpenses();
        Task<Expenses> GetExpense(int id);
        void CreateExpense(Expenses Expense);
        void UpdateExpense(Expenses Expense);
        Task DeleteExpense(Expenses Expense);
        Task Save();
        bool ExpenseExists(int id);
    }
}
