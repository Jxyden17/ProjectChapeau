using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Validation.Interfaces;

namespace ProjectChapeau.Validation
{
    public class TableEditValidator : ITableEditValidator
    {
        public TableValidationResult ValidateTableEdit(TableEditViewModel TableEdit, TableEditViewModel tableEditViewModel)
        {
            if (TableEdit == null || tableEditViewModel.orderId == null)
                return new TableValidationResult(false, "Table or Order not found. Try again.");

            if (TableEdit.currentOrderStatus == null || TableEdit.currentOrderStatus == OrderStatus.Completed)
                return new TableValidationResult(true); // No order active, can proceed to table update

            OrderStatus? requestedStatus = tableEditViewModel.currentOrderStatus;
            OrderStatus currentStatus = TableEdit.currentOrderStatus.Value;

            bool isTableStatusChanging = TableEdit.isOccupied != tableEditViewModel.isOccupied;
            bool isOrderStatusChanging = currentStatus != requestedStatus;

            if (isOrderStatusChanging)
            {
                if (requestedStatus == OrderStatus.Served && currentStatus != OrderStatus.ReadyToBeServed)
                    return new TableValidationResult(false, "You can only set the status to Served if the current order status is ReadyToBeServed.");

                if (currentStatus == OrderStatus.ReadyToBeServed && requestedStatus != OrderStatus.Served)
                    return new TableValidationResult(false, "You can only change 'ReadyToBeServed' orders to 'Served'.");
            }

            if (!isOrderStatusChanging && !isTableStatusChanging)
                return new TableValidationResult(false, "No changes detected in table or order status.");

            // If we reach here, all checks passed
            return new TableValidationResult(true);
        }
    }
}
