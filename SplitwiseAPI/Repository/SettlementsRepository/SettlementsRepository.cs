using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.SettlementsRepository
{
    public class SettlementsRepository : ISettlementsRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public SettlementsRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool SettlementExists(int id)
        {
            return context.Settlements.Any(e => e.Id == id);
        }

        public void CreateSettlement(Settlements Settlement)
        {
            context.Settlements.Add(Settlement);
        }

        public async Task DeleteSettlement(Settlements Settlement)
        {
            context.Settlements.Remove(Settlement);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public IEnumerable<Settlements> GetSettlements()
        {
            return context.Settlements.Include(p => p.Payee).Include(l => l.Payer);
        }

        public IEnumerable<Settlements> GetSettlementsByUserId(string id)
        {
            return context.Settlements.Include(p => p.Payee).Include(l => l.Payer).Where(s => s.PayeeId == id || s.PayerId == id).ToList();
        }

        public IEnumerable<Settlements> GetSettlementsByGroupId(int id)
        {
            return context.Settlements.Include(p => p.Payee).Include(l => l.Payer).Where(s => s.GroupId == id).ToList(); ;
        }

        public Task<Settlements> GetSettlement(int id)
        {
            return context.Settlements.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateSettlement(Settlements Settlement)
        {
            context.Entry(Settlement).State = EntityState.Modified;
        }
    }
}
