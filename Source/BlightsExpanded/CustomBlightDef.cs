using System;
using BlightsExpanded.Blights;
using Verse;

namespace BlightsExpanded
{
    public class CustomBlightDef : ThingDef
    {
        public int weight;
        public string letterLabel;
        public string letterText;

        private CustomBlight customBlight;
        public CustomBlight CustomBlight
        {
            get
            {
                customBlight = customBlight ?? (CustomBlight)Activator.CreateInstance(thingClass);
                return customBlight;
            }
        }
    }
}