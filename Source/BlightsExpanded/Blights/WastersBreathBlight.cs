using Verse;

namespace BlightsExpanded.Blights
{
    public class WastersBreathBlight : CustomBlight
    {
        public override void TickLong()
        {
            if (Rand.MTBEventOccurs(BlightDef.wastersBreathGasMtb, BlightDef.wastersBreathMtbUnitTicks, 2000))
            {
                GasUtility.AddGas(Position, Map, GasType.ToxGas, BlightDef.wastersBreathGasAmount);
            }

            base.TickLong();
        }
    }
}