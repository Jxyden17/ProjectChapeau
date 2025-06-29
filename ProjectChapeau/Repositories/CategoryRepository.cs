using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class CategoryRepository : ConnectionDatabase, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        { }

        private static Category ReadCategory(SqlDataReader reader)
        {
            // Shortened if-else statement (condition ? true-statement : false-statement)
            int categoryId = (int)reader["category_id"];
            string? categoryName = reader["category_name"] == DBNull.Value ? null : (string)reader["category_name"];

            return new Category(categoryId, categoryName);
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                                 FROM Category
                                 ORDER BY category_id";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Category category = ReadCategory(reader);
                    categories.Add(category);
                }
                reader.Close();

            }
            return categories;
        }
    }
}
