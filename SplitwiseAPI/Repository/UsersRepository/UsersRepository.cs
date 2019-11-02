using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Users> _userManager;

        public UsersRepository(SplitwiseAPIContext context, UserManager<Users> _userManager)
        {
            this.context = context;
            this._userManager = _userManager;
        }
        public IEnumerable<Users> GetUsers()
        {
            return _userManager.Users;
        }

        public Task<Users> GetUser(string id)
        {
            return _userManager.Users.Where(k => k.Id ==id).FirstAsync();
        }

        public Task<Users> GetUserByEmail(string email)
        {
            return _userManager.Users.Where(k => k.Email == email).SingleOrDefaultAsync();
        }

        public IEnumerable<Users> GetAllFriends(string id)
        {
            return context.UserFriendMappings.Where(u => u.UserId == id).Select(x => x.Friend);
        }

        public async Task CreateUser(Users user)
        {
            await _userManager.CreateAsync(user, user.Password);
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
            await _userManager.DeleteAsync(user);
        }

        public bool UserExists(string id)
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
