using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.GroupMemberMappingsRepository
{
    public interface IGroupMemberMappingsRepository
    {
        IEnumerable<GroupMemberMappings> GetGroupMemberMappings();
        Task<GroupMemberMappings> GetGroupMemberMapping(int id);
        Task CreateGroupMemberMapping(GroupMemberMappings GroupMemberMapping);
        void UpdateGroupMemberMapping(GroupMemberMappings GroupMemberMapping);
        Task DeleteGroupMemberMapping(GroupMemberMappings GroupMemberMapping);
        Task Save();
        bool GroupMemberMappingExists(int id);
    }
}
