using ProjectChapeau.Models;

namespace ProjectChapeau.Views.ViewModel
{
    public class MenusOverviewViewModel
    {
        public List<Menu>? Menus { get; set; }

        public MenusOverviewViewModel()
        {
        }
        public MenusOverviewViewModel(List<Menu>? menus)
        {
            Menus = menus;
        }
    }
}
