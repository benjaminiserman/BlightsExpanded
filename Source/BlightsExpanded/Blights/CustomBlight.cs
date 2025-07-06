using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlightsExpanded.Blights
{
    public abstract class CustomBlight : Blight
    {
        public CustomBlightDef BlightDef => (CustomBlightDef)def;

        public virtual void SpawnBlight(Plant plant)
        {
            if (plant.Blighted)
            {
                return;
            }

            GenSpawn.Spawn(def, plant.Position, plant.Map);
        }

        // this runs regardless of ShouldBlockGrowth and ShouldBlockYield
        public virtual void OnHarvest(float statValue, Pawn actor, JobDriver jobDriver)
        {
        }

        public bool ShouldBlockGrowth() => BlightDef.blockGrowth;
        public float AdjustGrowthRate(float growthRate) => growthRate * BlightDef.growthMultiplier;
        public bool ShouldBlockYield() => BlightDef.blockYield;
        public int AdjustYieldAmount(int yield) => (int)Math.Round(yield * BlightDef.yieldMultiplier);
    }
}