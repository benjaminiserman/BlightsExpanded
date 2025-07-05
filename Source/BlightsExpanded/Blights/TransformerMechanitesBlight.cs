using System;
using Verse;
using Verse.AI;

namespace BlightsExpanded.Blights
{
    public class TransformerMechanitesBlight : CustomBlight
    {
        public override void OnHarvest(float statValue, Pawn actor, JobDriver jobDriver)
        {
            if (Rand.Chance(BlightDef.transformerMechanitesDropChance))
            {
                if (BlightDef.transformerMechanitesDrops.TryRandomElementByWeight(x => x.weight, out var drop))
                {
                    var thing = ThingMaker.MakeThing(drop.thingDef);
                    thing.stackCount = (int)Math.Round(drop.count.RandomInRange * Plant.Growth);
                    if (stackCount > 0)
                    {
                        GenPlace.TryPlaceThing(thing, actor.Position, actor.Map, ThingPlaceMode.Near);
                    }
                }
            }

            base.OnHarvest(statValue, actor, jobDriver);
        }
    }
}