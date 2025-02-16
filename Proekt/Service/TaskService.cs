using Proekt.Repository;
using Proekt.Entites;
using TaskE = Proekt.Entites.TaskE;
using System.Data;
using Proekt.Models;
namespace Proekt.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;
        public TaskService(ITaskRepository taskRepository)
        {
            repository = taskRepository;
        }
        public void CreateTask(TaskE task, int userId)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.UserId = userId; // Устанавливаем userId из токена
            repository.CreateTask(task, userId);
        }

        public void DeleteTask(int id,int userId)
        {
            var task = repository.GetTaskById(id,userId);
            if(task == null)
            {
                throw new Exception("Task not found");
            }
            if (task.UserId != userId)
            {
                throw new Exception("User does not own this task.");
            }
            repository.DeleteTask(id,userId);
        }

    
        public IEnumerable<TaskE> GetAllTasks(int userId)
        {
            return repository.GetAllTask(userId);
        }
        public TaskE GetTaskByTitle(string title)
        {
            return repository.GetTaskByTitle(title);
        }

        public TaskE GetTaskById(int id,int userId)
        {
            var task = repository.GetTaskById(id,userId);
            if(task == null)
            {
                throw new Exception("Task not found");
            }
            return task;
        }

        public void MarkTaskAsCompleted(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(TaskE task,int userId)
        {
            var existingTask = repository.GetTaskById(task.Id,userId);
            if (existingTask == null)
            {
                throw new Exception("task not found");

            }
            if (existingTask.UserId != userId)
            {
                throw new Exception("User does not own this task.");
            }
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.IsCompleted = task.IsCompleted;
            repository.UpdateTask(existingTask,userId);
        }
        public void AssignCategoryToTask(int taskId, int categoryId, int userId)
        {
            var task = repository.GetTaskById(taskId,userId);

            if (task == null)
            {
                throw new Exception("Задача не найдена.");
            }

            if (task.UserId != userId)
            {
                throw new Exception("У вас нет прав для изменения этой задачи.");
            }

            repository.AssignCategoryToTask(taskId, categoryId,userId);
        }
        public IEnumerable<GroupTaskE> GetGroupTasks(int groupId,int id)
        {
            return repository.GetGroupTasks(groupId,id);
        }

        public void CreateGroupTask(GroupTaskE task,int id)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            repository.CreateGroupTask(task,id);
        }

        public GroupTaskE GetGroupTaskById(int taskId,int id,int groupId)
        {
            return repository.GetGroupTaskById(taskId,id,groupId);
        }

        public void UpdateGroupTask(GroupTaskE task,int id)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            repository.UpdateGroupTask(task,id);
        }

        public void DeleteGroupTask(int taskId,int id,int GroupId)
        {
            repository.DeleteGroupTask(taskId,id,GroupId);
        }
    }
}
