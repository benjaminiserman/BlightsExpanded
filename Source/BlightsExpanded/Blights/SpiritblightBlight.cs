using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace BlightsExpanded.Blights
{
    public class SpiritblightBlight : CustomBlight
    {
        public static HashSet<SpiritblightBlight> tickedBlights = new HashSet<SpiritblightBlight>();
        public static long lastGameTick = 0;

        public void TriggerIncident()
        {
            var parms = new IncidentParms
            {
                target = Map,
                points = StorytellerUtility.DefaultThreatPointsNow(Map),
                forced = true
            };

            // if >50% of colonists believe in animal personhood, farm animals wander in
            if (ModsConfig.IdeologyActive)
            {
                var colonistCount = Map.PlayerPawnsForStoryteller.Count(p => p.IsColonist);
                var veganCount = Map.PlayerPawnsForStoryteller
                    .Count(p => p.IsColonist
                                && p.ideo.Ideo.memes != null
                                && p.ideo.Ideo.memes.Any(meme => meme.defName == "AnimalPersonhood"));

                if (veganCount >= colonistCount / 2)
                {
                    Current.Game.storyteller.incidentQueue.Add(
                        IncidentDefOf.FarmAnimalsWanderIn,
                        Find.TickManager.TicksGame,
                        parms
                    );
                    return;
                }
            }

            // otherwise, manhunter pack
            Current.Game.storyteller.incidentQueue.Add(IncidentDefOf.ManhunterPack, Find.TickManager.TicksGame, parms);
        }

        public override void TickLong()
        {
            if (tickedBlights.Contains(this))
            {
                lastGameTick = Find.TickManager.TicksGame;

                if (Rand.MTBEventOccurs(
                        BlightDef.spiritblightEventMtbFor100 * 10 / (float)Math.Sqrt(tickedBlights.Count),
                        BlightDef.spiritblightMtbUnitTicks,
                        2000))
                {
                    TriggerIncident();
                }

                tickedBlights.Clear();
            }

            tickedBlights.Add(this);
            base.TickLong();
        }
    }
}