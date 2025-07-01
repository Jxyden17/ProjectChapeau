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
            List<Category> categories = [];

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT category_id, category_name
                                 FROM Category
                                 ORDER BY category_id";
                SqlCommand command = new(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(ReadCategory(reader));
                    }
                }
            }
            return categories;
        }

        public Category? GetCategoryById(int categoryId)
        {
            Category? category = null;

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT category_id, category_name
                                 FROM Category
                                 WHERE category_id = @CategoryId";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@CategoryId", categoryId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = ReadCategory(reader);
                    }
                }
            }
            return category;
        }
    }
}