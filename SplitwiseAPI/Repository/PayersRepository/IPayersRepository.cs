using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.PayersRepository
{
    public interface IPayersRepository
    {
        IEnumerable<Payers> GetPayers();
        IEnumerable<Payers> GetPayersByExpenseId(int id);
        IEnumerable<Payers> GetPayersByPayerId(string id);
        Task<Payers> GetPayer(int id);
        void CreatePayer(Payers Payer);
        void UpdatePayer(Payers Payer);
        Task DeletePayer(Payers Payer);
        Task Save();
        bool PayerExists(int id);
    }
}
