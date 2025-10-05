namespace GanhoDeCapital.Domain.Entities;

public class StockOperation
{
    public OperationType Operation { get; private set; }
    public decimal UnitCost { get; private set; }
    public int Quantity { get; private set; }

    public StockOperation(OperationType operation, decimal unitCost, int quantity)
    {
        Operation = operation;
        UnitCost = unitCost;
        Quantity = quantity;
    }
}

public enum OperationType
{
    Buy,
    Sell
}