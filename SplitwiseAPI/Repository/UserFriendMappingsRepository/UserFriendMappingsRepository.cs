using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Repository.UserFriendMappingsRepository
{
    public class UserFriendMappingsRepository : IUserFriendMappingsRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public UserFriendMappingsRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public void CreateUserFriendMapping(UserFriendMappings UserFriendMapping)
        {
            context.UserFriendMappings.Add(UserFriendMapping);
        }

        public async Task DeleteUserFriendMapping(UserFriendMappings UserFriendMapping)
        {
            var x = await context.UserFriendMappings.Where(k => k.UserId == UserFriendMapping.FriendId && k.FriendId == UserFriendMapping.UserId).FirstOrDefaultAsync();
            context.UserFriendMappings.Remove(UserFriendMapping);
            context.UserFriendMappings.Remove(x);
        }

        public async Task DeleteUserFriendMappingByIds(string id1, string id2)
        {
            var x = await context.UserFriendMappings.Where(k => k.UserId == id1 && k.FriendId == id2).FirstOrDefaultAsync();
            var y = await context.UserFriendMappings.Where(k => k.UserId == id2 && k.FriendId == id1).FirstOrDefaultAsync();
            var z = await context.UserFriendMappings.ToListAsync();
            System.Diagnostics.Debug.Write("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            System.Diagnostics.Debug.Write(id1);
            System.Diagnostics.Debug.Write(id2);
            System.Diagnostics.Debug.Write("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            System.Diagnostics.Debug.Write(x.Id);
            System.Diagnostics.Debug.Write(y.Id);

            context.UserFriendMappings.Remove(x);
            context.UserFriendMappings.Remove(y);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<UserFriendMappings> GetUserFriendMapping(int id)
        {
            return context.UserFriendMappings.FindAsync(id);
        }

        public IEnumerable<UserFriendMappings> GetUserFriendMappings()
        {
            System.Diagnostics.Debug.WriteLine("ASJKHDSJKHSDK");
            return context.UserFriendMappings;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateUserFriendMapping(UserFriendMappings UserFriendMapping)
        {
            context.Entry(UserFriendMapping).State = EntityState.Modified;
        }

        public bool UserFriendMappingExists(string id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
