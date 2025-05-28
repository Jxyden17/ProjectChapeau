using ProjectChapeau.Models;

namespace ProjectChapeau.Models.ViewModel
{
    public class MenusOverviewViewModel
    {
        public IEnumerable<Menu>? Menus { get; set; }

        public MenusOverviewViewModel()
        {
        }
        public MenusOverviewViewModel(IEnumerable<Menu>? menus)
        {
            Menus = menus;
        }
    }
}
