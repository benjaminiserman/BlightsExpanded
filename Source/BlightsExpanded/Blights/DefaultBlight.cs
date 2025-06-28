using RimWorld;

namespace BlightsExpanded.Blights
{
    public class DefaultBlight : CustomBlight
    {
        public override void SpawnBlight(Plant plant)
        {
            plant.CropBlighted();
        }

        public override bool ShouldBlockGrowth() => true;
        public override bool ShouldBlockYield() => true;
    }
}