using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.SettlementsRepository
{
    public interface ISettlementsRepository
    {
        IEnumerable<Settlements> GetSettlements();
        IEnumerable<Settlements> GetSettlementsByUserId(string id);
        IEnumerable<Settlements> GetSettlementsByGroupId(int id);
        Task<Settlements> GetSettlement(int id);
        void CreateSettlement(Settlements Settlement);
        void UpdateSettlement(Settlements Settlement);
        Task DeleteSettlement(Settlements Settlement);
        Task Save();
        bool SettlementExists(int id);
    }
}
