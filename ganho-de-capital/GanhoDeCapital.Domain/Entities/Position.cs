using System;

namespace GanhoDeCapital.Domain.Entities;

public class Position
{
    public decimal WeightedAverageCost { get; private set; }
    public int TotalShares { get; private set; }
    public decimal AccumulatedLoss { get; private set; }

    public decimal TotalCostBefore { get; private set; }

    public void AddShares(int quantity)
    {
        TotalShares += quantity;
    }
    public void RemoveShares(int quantity)
    {
        TotalShares -= quantity;
    }

    public void UpdateTotalCostBefore()
    {
        TotalCostBefore = WeightedAverageCost * TotalShares;
    }

    public void CalculateWeightedAveragePrice(decimal totalCostOperation)
    {
        if (TotalShares > 0)
            WeightedAverageCost = (TotalCostBefore + totalCostOperation) / TotalShares;
    }

    public void IncreaseAccumulatedLoss(decimal profit)
    {
        AccumulatedLoss += Math.Abs(profit);
    }
    public void ReduceAccumulatedLoss(decimal profit)
    {
        AccumulatedLoss -= profit;
    }

    internal void ResetAccumulatedLoss()
    {
        AccumulatedLoss = 0;
    }
}