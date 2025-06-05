using ProjectChapeau.Models;

namespace ProjectChapeau.Models.ViewModel
{
    public class MenuItemOverviewViewModel
    {
        public MenuItem? MenuItem { get; set; }
        public MenuItemOverviewViewModel()
        {

        }
        public MenuItemOverviewViewModel(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
