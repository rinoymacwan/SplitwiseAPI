using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Repository.ActivitiesRepository
{
    public class ActivitiesRepository : IActivitiesRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public ActivitiesRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool ActivityExists(int id)
        {
            return context.Activities.Any(e => e.Id == id);
        }

        public void CreateActivity(Activities Activity)
        {
            context.Activities.Add(Activity);
        }

        public async Task DeleteActivity(Activities Activity)
        {
            context.Activities.Remove(Activity);
        }
        public async Task DeleteAllActivities(int id)
        {
            context.Activities.RemoveRange(context.Activities.Where(k => k.UserId == id));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Activities> GetActivities()
        {
            return context.Activities;
        }

        public Task<Activities> GetActivity(int id)
        {
            return context.Activities.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateActivity(Activities Activity)
        {
            context.Entry(Activity).State = EntityState.Modified;
        }
    }
}
