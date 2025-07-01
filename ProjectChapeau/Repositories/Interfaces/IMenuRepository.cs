using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        List<int> GetAllMenuIds();
        Menu GetMenuById(int menuId);
    }
}
