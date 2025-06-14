namespace ProjectChapeau.Models
{
    public class TableValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public TableValidationResult(bool isValid, string errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
