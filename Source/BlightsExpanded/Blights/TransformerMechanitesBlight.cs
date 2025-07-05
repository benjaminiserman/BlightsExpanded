using Verse;
using Verse.AI;

namespace BlightsExpanded.Blights
{
    public class TransformerMechanitesBlight : CustomBlight
    {
        public override bool ShouldBlockYield() => true;

        public override void OnHarvest(float statValue, Pawn actor, JobDriver jobDriver)
        {


            base.OnHarvest(statValue, actor, jobDriver);
        }
    }
}