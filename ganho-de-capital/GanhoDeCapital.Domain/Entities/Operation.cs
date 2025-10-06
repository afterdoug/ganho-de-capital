namespace GanhoDeCapital.Domain.Entities;

public class Operation
{
    public OperationType OperationType { get; private set; }
    public decimal UnitCost { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalCostOperation => UnitCost * Quantity;

    public Operation(OperationType operation, decimal unitCost, int quantity)
    {
        OperationType = operation;
        UnitCost = unitCost;
        Quantity = quantity;
    }
}

public enum OperationType
{
    Buy,
    Sell
}