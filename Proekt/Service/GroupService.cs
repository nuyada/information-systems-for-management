using Proekt.Models;
using Proekt.Repository;

namespace Proekt.Service
{
    public class GroupService
    {
        private readonly GroupRepo repository;
        public GroupService(GroupRepo Repository)
        {
            repository = Repository;
        }
        public int CreateGroup(GroupE group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return repository.CreateGroup(group);
        }
        public IEnumerable<GroupE> GetUserGroups(int userId)
        {
            return repository.GetUserGroups(userId);
        }
        public GroupE GetGroupById(int groupId, int userId)
        {
            return repository.GetGroupById(groupId, userId);
        }

        public void AddUserToGroup(int userId, int groupId, int currentUserId)
        {
            repository.AddUserToGroup(userId, groupId, currentUserId);
        }

        public void DeleteGroup(int groupId, int userId)
        {
            repository.DeleteGroup(groupId, userId);
        }
    }
}
