using BlightsExpanded.Blights;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlightsExpanded
{
    public static class CustomBlightHelper
    {
        public static CustomBlightDef PickBlight()
        {
            DefDatabase<CustomBlightDef>.AllDefsListForReading.TryRandomElementByWeight(
                def => def.weight,
                out var result
            );
            return result;
        }

        public static bool BlockHarvestForBlight(Plant plant)
        {
            if (!plant.Blighted)
            {
                return false;
            }

            if (plant.Blight is CustomBlight blight)
            {
                return blight.ShouldBlockYield();
            }

            return true;
        }

        public static bool BlockGrowthForBlight(Plant plant)
        {
            if (!plant.Blighted)
            {
                return false;
            }

            if (plant.Blight is CustomBlight blight)
            {
                return blight.ShouldBlockGrowth();
            }

            return true;
        }

        public static void BlightOnHarvest(Plant plant, float statValue, Pawn actor, JobDriver jobDriver)
        {
            if (plant.Blight is CustomBlight blight)
            {
                blight.OnHarvest(statValue, actor, jobDriver);
            }
        }
    }
}