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

            // If no active order at all
            if (CurrentTableOrder.Order == null || CurrentTableOrder.Order.OrderStatus == OrderStatus.Completed)
            {
                bool updateTable = CurrentTableOrder.Table.IsOccupied != EditedTableOrder.Table.IsOccupied;
                if (!updateTable)
                    return new TableValidationResult(false, "No changes detected in table status.", false, false);
                return new TableValidationResult(true, null, updateTable, false);
            }

            OrderStatus? requestedStatus = EditedTableOrder.Order.OrderStatus;
            OrderStatus currentStatus = CurrentTableOrder.Order.OrderStatus;

            bool isTableStatusChanging = CurrentTableOrder.Table.IsOccupied != EditedTableOrder.Table.IsOccupied;
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
