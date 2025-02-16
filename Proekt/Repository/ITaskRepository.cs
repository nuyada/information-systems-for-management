using Proekt.Entites;
using Proekt.Models;
using TaskE = Proekt.Entites.TaskE;

namespace Proekt.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<TaskE> GetAllTask(int userId);
        TaskE GetTaskById(int id, int userId);
        void CreateTask(TaskE task,int userId);
        void UpdateTask(TaskE task,int userId);
        void DeleteTask(int id,int userId);
        void AssignCategoryToTask(int taskId, int categoryId,int userId);
        public TaskE GetTaskByTitle(string title);
        IEnumerable<GroupTaskE> GetGroupTasks(int groupId,int id);
        void CreateGroupTask(GroupTaskE task,int id);
        GroupTaskE GetGroupTaskById(int taskId,int id,int groupId);
        void UpdateGroupTask(GroupTaskE task,int id);
        void DeleteGroupTask(int taskId,int id,int GroupeId);
       

    }
}
