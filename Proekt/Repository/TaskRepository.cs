using Microsoft.Data.SqlClient;
using Proekt.Entites;
using Proekt.SQL_DB;
using System;
namespace Proekt.Repository
{
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using Proekt.Models;

    public class TaskRepository : ITaskRepository
    {
        private readonly MainConnector _connector;
        private bool IsUserInGroup(int userId, int groupId)
        {
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT COUNT(*) FROM UserGroups WHERE UserId = @UserId AND GroupId = @GroupId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public TaskRepository(MainConnector connector)
        {
            _connector = connector;
        }
        public TaskE GetTaskByTitle(string title)
        {
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT * FROM Tasks WHERE Title = @Title", connection))
            {
                command.Parameters.AddWithValue("@Title", title);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TaskE
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            DueDate = (DateTime)reader["DueDate"],
                            IsCompleted = (bool)reader["IsCompleted"],
                            UserId = (int)reader["UserId"],
                        };
                    }
                }
            }
            return null;
        }

        public IEnumerable<TaskE> GetAllTask(int userId)
        {
            var tasks = new List<TaskE>();
            var connection = _connector.GetConnection();

            // Измененный запрос: добавлен фильтр по userId
            using (var command = new SqlCommand("SELECT * FROM Tasks WHERE UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")), // Для ассоциации с пользователем
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }

            return tasks;
        }

        public TaskE GetTaskById(int id, int userId)
        {
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id AND UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TaskE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        };
                    }
                    else
                    {
                        // Если задача не найдена или она не принадлежит текущему пользователю,
                        // можно выбросить исключение или вернуть null.
                        return null;
                    }
                }
            }
        }

        public void CreateTask(TaskE task, int userId)
        {
            task.UserId = userId;
            var connection = _connector.GetConnection();

            using (var command = new SqlCommand("INSERT INTO Tasks (Title, Description, DueDate, IsCompleted, UserId,CategoryId) VALUES (@Title, @Description, @DueDate, @IsCompleted,@UserId,@CategoryId)", connection))
            {
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@DueDate", task.DueDate);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@CategoryId", task.CategoryId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateTask(TaskE task, int userId)
        {
            var connection = _connector.GetConnection();

            using (var command = new SqlCommand("UPDATE Tasks SET Title = @Title, Description = @Description, DueDate = @DueDate, IsCompleted = @IsCompleted WHERE Id = @Id AND UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@Id", task.Id);
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@DueDate", task.DueDate);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.Parameters.AddWithValue("@UserId", userId);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("Задача не найдена или принадлежит другому пользователю.");
                }

                command.ExecuteNonQuery();
            }
        }

        public void DeleteTask(int id,int userId)
        {
            var connection = _connector.GetConnection();

            using (var command = new SqlCommand("DELETE FROM Tasks WHERE Id = @Id AND UserId=@userId", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@userId", userId);

                command.ExecuteNonQuery();
            }
        }
        public void AssignCategoryToTask(int taskId, int categoryId,int userId)
        {
            var connection = _connector.GetConnection();

            using (var command = new SqlCommand("UPDATE Tasks SET CategoryId = @CategoryId WHERE Id = @TaskId AND UserId=@userId", connection))
            {
                command.Parameters.AddWithValue("@TaskId", taskId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@userId", userId);
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<GroupTaskE> GetGroupTasks(int groupId,int userId)
        {
            var tasks = new List<GroupTaskE>();
            var connection = _connector.GetConnection();
            if (!IsUserInGroup(userId, groupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }
            using (var command = new SqlCommand("SELECT * FROM GroupTasks WHERE GroupId = @GroupId", connection))
            {
                command.Parameters.AddWithValue("@GroupId", groupId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new GroupTaskE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                            CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                            GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }
            return tasks;
        }

        public void CreateGroupTask(GroupTaskE task,int id)
        {
            if (!IsUserInGroup(id, task.GroupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("INSERT INTO GroupTasks (Title, Description, DueDate, IsCompleted, CreatedBy, GroupId, CategoryId) VALUES (@Title, @Description, @DueDate, @IsCompleted, @CreatedBy, @GroupId, @CategoryId)", connection))
            {
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@DueDate", task.DueDate);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.Parameters.AddWithValue("@CreatedBy", task.CreatedBy);
                command.Parameters.AddWithValue("@GroupId", task.GroupId);
                command.Parameters.AddWithValue("@CategoryId", task.CategoryId );
                command.ExecuteNonQuery();
            }
        }

        public GroupTaskE GetGroupTaskById(int taskId,int id,int GroupId)
        {
            if (!IsUserInGroup(id, GroupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT * FROM GroupTasks WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", taskId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GroupTaskE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                            CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                            GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        };
                    }
                }
            }
            return null;
        }

        public void UpdateGroupTask(GroupTaskE task,int id)
        {
            if (!IsUserInGroup(id, task.GroupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("UPDATE GroupTasks SET Title = @Title, Description = @Description, DueDate = @DueDate, IsCompleted = @IsCompleted, CategoryId = @CategoryId WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", task.Id);
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@DueDate", task.DueDate);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.Parameters.AddWithValue("@CategoryId", task.CategoryId);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteGroupTask(int taskId,int id,int GroupId)
        {
            if (!IsUserInGroup(id, GroupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("DELETE FROM GroupTasks WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", taskId);
                command.ExecuteNonQuery();
            }
        }
    }

}
