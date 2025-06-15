using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Validation.Interfaces;

namespace ProjectChapeau.Validation
{
    public class TableEditValidator : ITableEditValidator
    {
        public TableValidationResult ValidateTableEdit(TableEditViewModel CurrentTableOrder, TableEditViewModel EditedTableOrder)
        {
            if (CurrentTableOrder == null)
                return new TableValidationResult(false, "Table or Order not found. Try again.", false, false);

            // If no active order, only table status can be changed
            if (CurrentTableOrder.currentOrderStatus == null || CurrentTableOrder.currentOrderStatus == OrderStatus.Completed)
            {
                bool updateTable = CurrentTableOrder.isOccupied != EditedTableOrder.isOccupied;
                if (!updateTable)
                    return new TableValidationResult(false, "No changes detected in table status.", false, false);
                return new TableValidationResult(true, null, updateTable, false);
            }

            OrderStatus? requestedStatus = EditedTableOrder.currentOrderStatus;
            OrderStatus currentStatus = CurrentTableOrder.currentOrderStatus.Value;

            bool isTableStatusChanging = CurrentTableOrder.isOccupied != EditedTableOrder.isOccupied;
            bool isOrderStatusChanging = currentStatus != requestedStatus;

            if (isTableStatusChanging)
                return new TableValidationResult(false, "Cannot change occupied status while an order is active.", false, false);

            if (isOrderStatusChanging)
            {
                if (requestedStatus == OrderStatus.Served && currentStatus != OrderStatus.ReadyToBeServed)
                    return new TableValidationResult(false, "You can only set the status to Served if the current order status is ReadyToBeServed.", false, false);

                if (currentStatus == OrderStatus.ReadyToBeServed && requestedStatus != OrderStatus.Served)
                    return new TableValidationResult(false, "You can only change 'ReadyToBeServed' orders to 'Served'.", false, false);
            }

            if (!isOrderStatusChanging && !isTableStatusChanging)
                return new TableValidationResult(false, "No changes detected in table or order status.", false, false);

            // All checks passed, indicate what should be updated
            return new TableValidationResult(
                true,
                null,
                isTableStatusChanging,
                isOrderStatusChanging
            );
        }
    }
}
