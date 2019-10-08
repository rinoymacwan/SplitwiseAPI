using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.UsersRepository
{
    public class UsersRepository : IUsersRepository, IDisposable
    {
        private SplitwiseAPIContext context;

        public UsersRepository(SplitwiseAPIContext context)
        {
            this.context = context;
        }
        public IEnumerable<Users> GetUsers()
        {
            return context.Users;
        }

        public Task<Users> GetUser(int id)
        {
            return context.Users.FindAsync(id);
        }

        public IEnumerable<Users> GetAllFriends(int id)
        {
            return context.UserFriendMappings.Where(u => u.UserId == id).Select(x => x.Friend);
        }

        public void CreateUser(Users user)
        {
            context.Users.Add(user);
        }

        public void UpdateUser(Users user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public void DeleteUser(Users user)
        {
            context.Users.Remove(user);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
        

        Task<Users> IUsersRepository.DeleteUser(Users user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
