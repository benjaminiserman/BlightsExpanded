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

        public virtual bool ShouldBlockGrowth() => false;
        public virtual float AdjustGrowthRate(float growthRate) => growthRate;
        public virtual bool ShouldBlockYield() => false;
        public virtual int AdjustYieldAmount(int yield) => yield;
    }
}