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

        public Task<Users> GetUserByEmail(string email)
        {
            return context.Users.Where(k => k.Email == email).SingleOrDefaultAsync();
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


        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
        

        async Task IUsersRepository.DeleteUser(Users user)
        {
            context.Users.Remove(user);
        }

        public bool UserExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }

        public void Dispose()
        {
           // throw new NotImplementedException();
        }
        public async Task<Users> Authenticate(Users user)
        {
            var x = await context.Users.Where(k => k.Email == user.Email && k.Password == user.Password).SingleOrDefaultAsync();
            if(x != null)
            {
                return x;
            } else
            {
                return new Users();
            }
        }
    }
}
