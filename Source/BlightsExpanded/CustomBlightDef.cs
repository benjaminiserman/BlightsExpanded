using System;
using BlightsExpanded.Blights;
using Verse;

namespace BlightsExpanded
{
    public class CustomBlightDef : Def
    {
        public int weight;
        public Type workerClass;

        private CustomBlight customBlight;
        public CustomBlight CustomBlight
        {
            get
            {
                customBlight = customBlight ?? (CustomBlight)Activator.CreateInstance(workerClass);
                return customBlight;
            }
        }
    }
}