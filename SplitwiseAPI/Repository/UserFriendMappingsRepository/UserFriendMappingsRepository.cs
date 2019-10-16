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

        public bool UserFriendMappingExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
