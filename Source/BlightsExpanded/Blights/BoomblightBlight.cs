using System;
using RimWorld;
using Verse;

namespace BlightsExpanded.Blights
{
    public class BoomblightBlight : CustomBlight
    {
        public ThingComp ExplosiveComp = new CompExplosive();

        public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            if (dinfo.Def == DamageDefOf.Flame)
            {
                GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, DamageDefOf.Flame);
                GenExplosion.DoExplosion(
                    Position,
                    Map,
                    BlightDef.boomblightBlastRadius,
                    DamageDefOf.Flame,
                    null,
                    preExplosionSpawnThingDef: ThingDefOf.Filth_Fuel,
                    preExplosionSpawnChance: 1f
                );
            }

            absorbed = false;
        }
    }
}