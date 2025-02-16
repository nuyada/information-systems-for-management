using Microsoft.Data.SqlClient;
using Proekt.Models;
using Proekt.SQL_DB;

namespace Proekt.Repository
{
    public class GroupRepo
    {
        private readonly MainConnector _connector;
        public GroupRepo(MainConnector connector)
        {
            _connector = connector;
        }
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
        public int CreateGroup(GroupE group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var connection = _connector.GetConnection();

            // Создаем группу и получаем её ID
            using (var command = new SqlCommand("INSERT INTO Groups (Name, Description, CreatedBy) OUTPUT INSERTED.Id VALUES (@Name, @Description, @CreatedBy)", connection))
            {
                command.Parameters.AddWithValue("@Name", group.Name);
                command.Parameters.AddWithValue("@Description", group.Description);
                command.Parameters.AddWithValue("@CreatedBy", group.CreatedBy);

                int groupId = (int)command.ExecuteScalar(); // Получаем ID созданной группы

                // Добавляем создателя группы в UserGroups
                using (var addUserCommand = new SqlCommand("INSERT INTO UserGroups (UserId, GroupId) VALUES (@UserId, @GroupId)", connection))
                {
                    addUserCommand.Parameters.AddWithValue("@UserId", group.CreatedBy);
                    addUserCommand.Parameters.AddWithValue("@GroupId", groupId);
                    addUserCommand.ExecuteNonQuery();
                }

                return groupId; // Возвращаем ID созданной группы
            }
        }
        public GroupE GetGroupById(int groupId, int userId)
        {
            // Проверяем, является ли пользователь участником группы
            if (!IsUserInGroup(userId, groupId))
            {
                throw new UnauthorizedAccessException("You are not a member of this group.");
            }

            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT * FROM Groups WHERE Id = @GroupId", connection))
            {
                command.Parameters.AddWithValue("@GroupId", groupId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GroupE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"))
                        };
                    }
                }
            }
            return null;
        }
        public IEnumerable<GroupE> GetUserGroups(int userId)
        {
            var groups = new List<GroupE>();
            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("SELECT g.* FROM UserGroups ug INNER JOIN Groups g ON ug.GroupId = g.Id WHERE ug.UserId = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new GroupE
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"))
                        });
                    }
                }
            }
            return groups;
        }
        public void AddUserToGroup(int userId, int groupId, int currentUserId)
        {
            // Проверяем, является ли текущий пользователь создателем группы
            GroupE group = GetGroupById(groupId, currentUserId);
            if (group == null || group.CreatedBy != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the creator of the group can add users.");
            }

            var connection = _connector.GetConnection();
            using (var command = new SqlCommand("INSERT INTO UserGroups (UserId, GroupId) VALUES (@UserId, @GroupId)", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteGroup(int groupId, int userId)
        {
            var connection = _connector.GetConnection();

            // Получаем создателя группы
            GroupE group = GetGroupById(groupId, userId);
            if (group == null || group.CreatedBy != userId)
            {
                throw new UnauthorizedAccessException("Only the creator of the group can delete it.");
            }

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Удаляем связи пользователей с группой
                    using (var cmdRemoveUsers = new SqlCommand("DELETE FROM UserGroups WHERE GroupId = @GroupId", connection, transaction))
                    {
                        cmdRemoveUsers.Parameters.AddWithValue("@GroupId", groupId);
                        cmdRemoveUsers.ExecuteNonQuery();
                    }

                    // Удаляем саму группу
                    using (var cmdDeleteGroup = new SqlCommand("DELETE FROM Groups WHERE Id = @GroupId", connection, transaction))
                    {
                        cmdDeleteGroup.Parameters.AddWithValue("@GroupId", groupId);
                        cmdDeleteGroup.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
