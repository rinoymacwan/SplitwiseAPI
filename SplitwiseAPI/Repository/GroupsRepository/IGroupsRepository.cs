using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.GroupsRepository
{
    public interface IGroupsRepository
    {
        IEnumerable<Groups> GetGroups();
        Task<Groups> GetGroup(int id);
        void CreateGroup(Groups Group);
        void UpdateGroup(Groups Group);
        Task DeleteGroup(Groups Group);
        Task Save();
        bool GroupExists(int id);
    }
}
