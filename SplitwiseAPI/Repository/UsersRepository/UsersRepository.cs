using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Utility;
using SplitwiseAPI.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.UsersRepository
{
    public class UsersRepository : IUsersRepository, IDisposable
    {
        private SplitwiseAPIContext context;
        private readonly UserManager<Users> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public UsersRepository(SplitwiseAPIContext context, UserManager<Users> _userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.context = context;
            this._userManager = _userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
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

        public async Task<bool> Login(Users credentials)
        {
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            var user = await _userManager.FindByEmailAsync(credentials.UserName);
            if(identity == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<string> GenerateJWT(Users credentials)
        {
            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            var user = await _userManager.FindByEmailAsync(credentials.UserName);
            var jwt = await Tokens.GenerateJwt(user, identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return jwt;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);
            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            var x = await _userManager.Users.ToListAsync();

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
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
