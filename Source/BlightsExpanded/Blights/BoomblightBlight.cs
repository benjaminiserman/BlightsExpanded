using System;
using RimWorld;
using Verse;

namespace BlightsExpanded.Blights
{
    public class BoomblightBlight : CustomBlight
    {
        public override int AdjustYieldAmount(int yield) => (int)Math.Round(yield * 1.2);
    }
}