using System;

namespace BlightsExpanded.Blights
{
    public class HypergrowthBlight : CustomBlight
    {
        public override float AdjustGrowthRate(float growthRate) => growthRate * 2;
        public override int AdjustYieldAmount(int yield) => (int)Math.Round(yield * 0.33f);
    }
}