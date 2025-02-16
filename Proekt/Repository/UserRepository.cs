using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using Proekt.Entites;
using Proekt.Hasher;
using Proekt.Models;
using Proekt.SQL_DB;
using System.Threading.Tasks;

namespace Proekt.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MainConnector connector;
        public UserRepository(MainConnector connector)
        {
            this.connector = connector;
        }
        public void CreateUser(Users users)
        {
            var connection = connector.GetConnection();
            using (var command = new SqlCommand("INSERT INTO Users (Name, LastName, MiddleName, Email, PasswordHash) VALUES (@Name, @LastName, @MiddleName, @Email, @PasswordHash)", connection))
            {
                command.Parameters.AddWithValue("@Name", users.Name);
                command.Parameters.AddWithValue("@LastName", users.LastName);
                command.Parameters.AddWithValue("@MiddleName", users.MiddleName);
                command.Parameters.AddWithValue("@Email", users.Email);
                command.Parameters.AddWithValue("@PasswordHash", users.PasswordHash);

                command.ExecuteNonQuery();
            }
        }
        public List<string> GetUserRoles(int userId)
        {
            var roles = new List<string>();
            using(var connection = connector.GetConnection())
            {
                var query = @"SELECT r.Name FROM UserRoles ur JOIN Roles r ON ur.RoleID = r.Id WHERE ur.UserID = @UserId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(reader.GetOrdinal("Name")));
                        }
                    }
                }
            }
            return roles;
        }
        public void AssingRoleToUser(int userId, int roleId)
        {
            using (var connection = connector.GetConnection())
            {
                var query = @"
                INSERT INTO UserRoles (UserId, RoleId)
                VALUES (@UserId, @RoleId)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int Id)
        {
            var connection = connector.GetConnection();

            using (var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", Id);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Users> GetAllUsers()
        {
            var users = new List<Users>();
            var connection = connector.GetConnection();

            using (var command = new SqlCommand("SELECT * FROM Users", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new Users
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = !reader.IsDBNull(reader.GetOrdinal("Name")) ? reader.GetString(reader.GetOrdinal("Name")) : string.Empty,
                            LastName = !reader.IsDBNull(reader.GetOrdinal("LastName")) ? reader.GetString(reader.GetOrdinal("LastName")) : string.Empty,
                            MiddleName = !reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? reader.GetString(reader.GetOrdinal("MiddleName")) : string.Empty,
                            PasswordHash = !reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? reader.GetString(reader.GetOrdinal("PasswordHash")) : string.Empty,
                            Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString(reader.GetOrdinal("Email")) : string.Empty
                        });
                    }
                }
            }

            return users;
        }

        public Users GetUserByEmail(string email)
        {
            Users user = null;
            var connection = connector.GetConnection();

            using (var command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                    }
                }
            }

            return user; ;
        }

        public Users GetUserById(int Id)
        {
            Users user = null;
            var connection = connector.GetConnection();

            using (var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", Id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                    }
                }
            }

            return user; ;
        }

        public void UpdateUser(UserModel users,int Id)
        {
            var query = @"
            UPDATE Users
            SET Name = @Name, LastName = @LastName, MiddleName = @MiddleName, Email = @Email, PasswordHash = @PasswordHash
            WHERE Id = @Id";

            using (var connection = connector.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                var hashedPassword = PasswordHasher.HashPassword(users.PasswordHash);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Name", users.Name);
                command.Parameters.AddWithValue("@LastName", users.LastName);
                command.Parameters.AddWithValue("@MiddleName", users.MiddleName);
                command.Parameters.AddWithValue("@Email", users.Email);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                command.ExecuteNonQuery();
            }
        }
    }
}
