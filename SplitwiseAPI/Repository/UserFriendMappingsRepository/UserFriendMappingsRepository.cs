using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.UsersRepository;

namespace SplitwiseAPI.Repository.UserFriendMappingsRepository
{
    public class UserFriendMappingsRepository : IUserFriendMappingsRepository, IDisposable
    {
        private SplitwiseAPIContext context;
        private readonly IUsersRepository _usersRepository;

        public UserFriendMappingsRepository(SplitwiseAPIContext context, IUsersRepository usersRepository)
        {
            this.context = context;
            this._usersRepository = usersRepository;
        }
        public void CreateUserFriendMapping(UserFriendMappings userFriendMapping)
        {
            UserFriendMappings otherEntry = new UserFriendMappings() { UserId = userFriendMapping.FriendId, FriendId = userFriendMapping.UserId };
            context.UserFriendMappings.Add(userFriendMapping);
            context.UserFriendMappings.Add(otherEntry);
        }
        public async Task<Boolean> CreateUserFriendMappingByEmail(string id, Users user)
        {
            string email = user.Email;
            var x = await _usersRepository.GetUserByEmail(email);
            if (x != null)
            {
                context.UserFriendMappings.Add(new UserFriendMappings() { UserId = id, FriendId = x.Id });
                context.UserFriendMappings.Add(new UserFriendMappings() { UserId = x.Id, FriendId = id });
                // _userFriendMappingsRepository.CreateUserFriendMapping(new UserFriendMappings() { UserId = id, FriendId = x.Id });
                //  _userFriendMappingsRepository.CreateUserFriendMapping(new UserFriendMappings() { UserId = x.Id, FriendId = id });
                await SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
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
