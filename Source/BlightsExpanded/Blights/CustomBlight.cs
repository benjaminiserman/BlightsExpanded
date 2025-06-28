using System;
using System.Reflection;
using RimWorld;
using Verse;
using Verse.AI;

namespace BlightsExpanded.Blights
{
    public abstract class CustomBlight : Blight
    {
        public abstract void BlightPlant(Plant plant);

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