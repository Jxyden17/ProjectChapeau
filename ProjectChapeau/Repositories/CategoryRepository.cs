using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        { }

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
