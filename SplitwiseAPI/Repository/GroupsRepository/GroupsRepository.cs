using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplitwiseAPI.DomainModel.Models;
using SplitwiseAPI.Models;
using SplitwiseAPI.Repository.ExpensesRepository;
using SplitwiseAPI.Repository.GroupMemberMappingsRepository;
using SplitwiseAPI.Repository.UserFriendMappingsRepository;

namespace SplitwiseAPI.Repository.GroupsRepository
{
    public class GroupsRepository : IGroupsRepository, IDisposable
    {
        private SplitwiseAPIContext context;
        private readonly IGroupMemberMappingsRepository _groupMemberMappingsRepository;
        private readonly IUserFriendMappingsRepository _userFriendMappingsRepository;
        private readonly IExpensesRepository _expensesRepository;
        public GroupsRepository(SplitwiseAPIContext _context, IGroupMemberMappingsRepository groupMemberMappingsRepository, IUserFriendMappingsRepository userFriendMappingsRepository, IExpensesRepository expensesRepository)
        {
            this.context = _context;
            this._groupMemberMappingsRepository = groupMemberMappingsRepository;
            this._userFriendMappingsRepository = userFriendMappingsRepository;
            this._expensesRepository = expensesRepository;
        }
        public bool GroupExists(int id)
        {
            return context.Groups.Any(e => e.Id == id);
        }

        public void CreateGroup(Groups Group)
        {
            context.Groups.Add(Group);
        }

        public async Task DeleteGroup(Groups Group)
        {
            await _groupMemberMappingsRepository.DeleteGroupMemberMappingByGroupId(Group.Id);
            await _expensesRepository.DeleteExpensesByGroupId(Group.Id);
            context.Groups.Remove(Group);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public IEnumerable<Groups> GetGroups()
        {
            return context.Groups;
        }
        public IEnumerable<Groups> GetGroupsByUserId(string id)
        {
            return _groupMemberMappingsRepository.GetGroupMemberMappings().Where(g => g.MemberId == id).Select(k => k.Group).ToList();
        }

        public Task<Groups> GetGroup(int id)
        {
            return context.Groups.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<GroupAndMembers> GetGroupWithDetails(int id)
        {
            var group =  await context.Groups.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            var members = _groupMemberMappingsRepository.GetGroupMemberMappings().Where(g => g.GroupId == id).Select(k => k.User).ToList();

            return new GroupAndMembers() { Group = group, Members = members };
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateGroup(Groups Group)
        {
            context.Entry(Group).State = EntityState.Modified;
        }
    }
}
