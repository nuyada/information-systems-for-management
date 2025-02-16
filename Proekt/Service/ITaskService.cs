using Proekt.Entites;
using Proekt.Models;
using TaskE = Proekt.Entites.TaskE;
namespace Proekt.Service
{
    public interface ITaskService
    {
        IEnumerable<TaskE> GetAllTasks(int userId); //получение всех задач
        TaskE GetTaskById(int id,int userId); //
        void CreateTask(TaskE task,int userId);
        void UpdateTask(TaskE task,int userId);
        void DeleteTask(int id,int userId);
        void MarkTaskAsCompleted(int id);
        void AssignCategoryToTask(int taskId, int categoryId, int userId);
        public TaskE GetTaskByTitle(string title);
        IEnumerable<GroupTaskE> GetGroupTasks(int groupId,int id);
        void CreateGroupTask(GroupTaskE task,int id);
        GroupTaskE GetGroupTaskById(int taskId,int id,int groupId);
        void UpdateGroupTask(GroupTaskE task,int id);
        void DeleteGroupTask(int taskId,int id,int GroupId);

    }
}
