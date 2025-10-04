using System;

namespace GanhoDeCapital.Domain.Entities
{
    public class StockOperation
    {
        public OperationType Operation { get; private set; }
        public decimal UnitCost { get; private set; }
        public int Quantity { get; private set; }

        public StockOperation(OperationType operation, decimal unitCost, int quantity)
        {
            if (unitCost <= 0)
                throw new ArgumentException("Unit cost must be greater than zero", nameof(unitCost));
            
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            Operation = operation;
            UnitCost = unitCost;
            Quantity = quantity;
        }

        public decimal TotalValue => UnitCost * Quantity;
    }

    public enum OperationType
    {
        Buy,
        Sell
    }
}