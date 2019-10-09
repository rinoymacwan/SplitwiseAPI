using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Repository.GroupsRepository
{
    public class GroupsRepository : IGroupsRepository, IDisposable
    {
        private SplitwiseAPIContext context;
        public GroupsRepository(SplitwiseAPIContext _context)
        {
            this.context = _context;
        }
        public bool GroupExists(int id)
        {
            return context.Groups.Any(e => e.Id == id);
        }

        public void CreateGroup(Groups Group)
        {
            context.Groups.Add(Group);
        }

        public async Task DeleteGroup(Groups Group)
        {
            context.Groups.Add(Group);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Groups> GetGroups()
        {
            return context.Groups;
        }

        public Task<Groups> GetGroup(int id)
        {
            return context.Groups.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateGroup(Groups Group)
        {
            context.Entry(Group).State = EntityState.Modified;
        }
    }
}
