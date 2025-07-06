using System.Linq;
using BlightsExpanded.Blights;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlightsExpanded
{
    public static class CustomBlightHelper
    {
        public static bool MayRequireFulfilled(CustomBlightDef def)
        {
            if (string.IsNullOrEmpty(def.mayRequire))
            {
                return true;
            }

            return LoadedModManager.RunningModsListForReading
                .Any(mod => mod.PackageId.EqualsIgnoreCase(def.mayRequire));
        }

        public static CustomBlightDef PickBlight()
        {
            DefDatabase<CustomBlightDef>.AllDefsListForReading
                .Where(MayRequireFulfilled)
                .TryRandomElementByWeight(
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