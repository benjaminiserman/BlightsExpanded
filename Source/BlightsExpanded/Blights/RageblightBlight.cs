using RimWorld;
using Verse;

namespace BlightsExpanded.Blights
{
    public class RageblightBlight : CustomBlight
    {
        public override void TickLong()
        {
            if (Rand.MTBEventOccurs(1000, 60_000, 2000))
            {
                var parms = new IncidentParms
                {
                    target = Map,
                    points = StorytellerUtility.DefaultThreatPointsNow(Map),
                    forced = true
                };

                Current.Game.storyteller.incidentQueue.Add(IncidentDefOf.ManhunterPack, Find.TickManager.TicksGame,
                    parms);
            }

            base.TickLong();
        }
    }
}