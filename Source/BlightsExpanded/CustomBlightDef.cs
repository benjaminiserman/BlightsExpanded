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

        public float yieldMultiplier;
        public float growthMultipler;
        public bool blockYield;
        public bool blockGrowth;

        // Blight-specific props
        public float boomblightBlastRadius;

        public float rageblightEventMtb;
        public float rageblightMtbUnitTicks;

        public float wastersBreathGasMtb;
        public float wastersBreathMtbUnitTicks;
        public float wastersBreathGasAmount;

        public float transformerMechanitesDropChance;
        public List<ThingCountWeightDef> transformerMechanitesDrops;

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