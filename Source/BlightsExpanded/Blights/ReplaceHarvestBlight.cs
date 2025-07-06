using System;
using Verse;
using Verse.AI;

namespace BlightsExpanded.Blights
{
    public class ReplaceHarvestBlight : CustomBlight
    {
        public override void OnHarvest(float statValue, Pawn actor, JobDriver jobDriver)
        {
            if (Rand.Chance(BlightDef.replaceHarvestChance))
            {
                if (BlightDef.replaceHarvestDrops.TryRandomElementByWeight(x => x.weight, out var drop))
                {
                    var thing = ThingMaker.MakeThing(drop.thingDef);
                    var adjustedStackCount = drop.count.RandomInRange * Plant.Growth;
                    thing.stackCount = (int)adjustedStackCount + (Rand.Chance(adjustedStackCount % 1) ? 1 : 0);
                    if (thing.stackCount > 0)
                    {
                        GenPlace.TryPlaceThing(thing, actor.Position, actor.Map, ThingPlaceMode.Near);
                    }
                }
            }

            base.OnHarvest(statValue, actor, jobDriver);
        }
    }
}