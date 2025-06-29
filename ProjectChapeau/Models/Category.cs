namespace ProjectChapeau.Models
{
    public class Category
    {

        public int CategoryId { get; set; }
        public string? CategoryName {  get; set; }
        public Category()
        {
        }
        public Category(int categoryId, string? categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
    }
}
