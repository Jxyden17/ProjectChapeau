namespace ProjectChapeau.Models
{
    public class TableValidationResult 
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public bool UpdateTable { get; set; }
        public bool UpdateOrder { get; set; }

        public TableValidationResult(bool isValid, string errorMessage = null, bool updateTable= false, bool updateOrder = false)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            UpdateTable = updateTable;
            UpdateOrder = updateOrder;
        }
    }
}
