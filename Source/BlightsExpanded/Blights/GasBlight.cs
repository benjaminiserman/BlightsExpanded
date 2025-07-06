using Verse;

namespace BlightsExpanded.Blights
{
    public class GasBlight : CustomBlight
    {
        public override void TickLong()
        {
            if (Rand.Chance(BlightDef.gasChance))
            {
                GasUtility.AddGas(Position, Map, BlightDef.gasType, BlightDef.gasAmount);
            }

            base.TickLong();
        }
    }
}