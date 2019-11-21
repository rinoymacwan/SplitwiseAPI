using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.UserFriendMappingsRepository
{
    public interface IUserFriendMappingsRepository
    {
        IEnumerable<UserFriendMappings> GetUserFriendMappings();
        Task<UserFriendMappings> GetUserFriendMapping(int id);
        void CreateUserFriendMapping(UserFriendMappings UserFriendMapping);
        Task<Boolean> CreateUserFriendMappingByEmail(string id, Users user);
        void UpdateUserFriendMapping(UserFriendMappings UserFriendMapping);
        Task DeleteUserFriendMapping(UserFriendMappings UserFriendMapping);
        Task DeleteUserFriendMappingByIds(string id1, string id2);
        Task SaveAsync();
        bool UserFriendMappingExists(string id);
    }
}
