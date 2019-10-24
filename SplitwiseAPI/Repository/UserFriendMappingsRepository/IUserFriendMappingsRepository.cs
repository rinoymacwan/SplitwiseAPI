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
        void UpdateUserFriendMapping(UserFriendMappings UserFriendMapping);
        Task DeleteUserFriendMapping(UserFriendMappings UserFriendMapping);
        Task DeleteUserFriendMappingByIds(int id1, int id2);
        Task SaveAsync();
        bool UserFriendMappingExists(int id);
    }
}
