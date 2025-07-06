using System;
using System.Collections.Generic;
using BlightsExpanded.Blights;
using Verse;

namespace BlightsExpanded
{
    public class CustomBlightDef : ThingDef
    {
        public float weight;
        public string letterLabel;
        public string letterText;

        public string mayRequire;

        public float yieldMultiplier;
        public float growthMultiplier;
        public bool blockYield;
        public bool blockGrowth;

        // Blight-specific props
        public float boomblightBlastRadius;

        public float spiritblightEventMtbFor100;
        public float spiritblightMtbUnitTicks;

        public float gasChance;
        public float gasAmount;
        public GasType gasType;

        public float replaceHarvestChance;
        public List<ThingCountWeightDef> replaceHarvestDrops;

        private CustomBlight customBlight;

        public CustomBlight CustomBlight
        {
            get
            {
                customBlight = customBlight ?? (CustomBlight)Activator.CreateInstance(thingClass);
                customBlight.def = this;
                return customBlight;
            }
        }
    }
}