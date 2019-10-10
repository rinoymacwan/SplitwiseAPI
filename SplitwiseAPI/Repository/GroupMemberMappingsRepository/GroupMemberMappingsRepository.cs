using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.GroupMemberMappingsRepository
{
    public class GroupMemberMappingsRepository : IGroupMemberMappingsRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public GroupMemberMappingsRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public bool GroupMemberMappingExists(int id)
        {
            return context.GroupMemberMappings.Any(e => e.Id == id);
        }

        public async Task CreateGroupMemberMapping(GroupMemberMappings GroupMemberMapping)
        {
            context.GroupMemberMappings.Add(GroupMemberMapping);
            await Save();
        }

        public async Task DeleteGroupMemberMapping(GroupMemberMappings GroupMemberMapping)
        {
            context.GroupMemberMappings.Add(GroupMemberMapping);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupMemberMappings> GetGroupMemberMappings()
        {
            return context.GroupMemberMappings.Include(k => k.User).Include(l => l.Group);
        }

        public Task<GroupMemberMappings> GetGroupMemberMapping(int id)
        {
            return context.GroupMemberMappings.FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateGroupMemberMapping(GroupMemberMappings GroupMemberMapping)
        {
            context.Entry(GroupMemberMapping).State = EntityState.Modified;
        }
    }
}
