using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAllMenuItems();
    }
}
