using Microsoft.Data.SqlClient;
using Proekt.Entites;
using Proekt.SQL_DB;

namespace Proekt.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private MainConnector _connector;
        public CategoryRepository(MainConnector connector)
        {
            _connector = connector;
        }
        public void CreateCategory(Category category)
        {
            var connection = _connector.GetConnection();
            using(var command = new SqlCommand("INSERT INTO Categories (Name) VALUES (@Name)", connection))
            {
                command.Parameters.AddWithValue("@Name", category.Name);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Category> GetAllCategoris()
        {
            var categories = new List<Category>();
            using (var connection = _connector.GetConnection())
            {
                var query = "SELECT * FROM Categories";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            });
                        }
                    }
                }
            }
            return categories;
        }

        public IEnumerable<TaskE> GetCategoryById(int CategoryId,int userId)
        {
            var tasks = new List<TaskE>();
            var connection = _connector.GetConnection();

            using (var command = new SqlCommand("SELECT * FROM Tasks WHERE CategoryId = @CategoryId AND UserId = @userId", connection))
            {
                command.Parameters.AddWithValue("@CategoryId", CategoryId);
                command.Parameters.AddWithValue("@userId",userId);
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
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }
            return tasks;
        }
    }
}
