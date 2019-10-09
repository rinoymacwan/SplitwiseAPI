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
            context.Settlements.Add(Settlement);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Settlements> GetSettlements()
        {
            return context.Settlements;
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
