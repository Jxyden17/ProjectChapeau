using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;

namespace ProjectChapeau.Validation.Interfaces
{
    public interface ITableEditValidator
    {
        TableValidationResult ValidateTableEdit(TableEditViewModel tableEdit, TableEditViewModel tableEditViewModel);
    }
}
