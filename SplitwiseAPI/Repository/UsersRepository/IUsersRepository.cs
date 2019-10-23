using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.UsersRepository
{
    public interface IUsersRepository : IDisposable
    {
        IEnumerable<Users> GetUsers();
        IEnumerable<Users> GetAllFriends(int id);
        Task<Users> GetUser(int id);
        void CreateUser(Users user); 
        void UpdateUser(Users user);
        Task DeleteUser(Users user);
        Task Save();
        bool UserExists(int id);
        Task<Users> Authenticate(Users user);
    }
}
