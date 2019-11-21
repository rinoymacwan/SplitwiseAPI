using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.PayeesRepository
{
    public interface IPayeesRepository
    {
        IEnumerable<Payees> GetPayees();
        IEnumerable<Payees> GetPayeesByExpenseId(int id);
        IEnumerable<Payees> GetPayeesByPayeeId(string id);
        Task<Payees> GetPayee(int id);
        void CreatePayee(Payees Payee);
        void UpdatePayee(Payees Payee);
        Task DeletePayee(Payees Payee);
        Task Save();
        bool PayeeExists(int id);
    }
}
