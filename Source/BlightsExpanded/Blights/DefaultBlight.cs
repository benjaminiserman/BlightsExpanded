using RimWorld;

namespace BlightsExpanded.Blights
{
    public class DefaultBlight : CustomBlight
    {
        public override void BlightPlant(Plant plant)
        {
            plant.CropBlighted();
        }

        public override bool ShouldBlockGrowth() => true;
        public override bool ShouldBlockYield() => true;
    }
}