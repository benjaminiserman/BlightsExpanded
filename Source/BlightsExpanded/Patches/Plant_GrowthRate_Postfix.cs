using BlightsExpanded.Blights;
using HarmonyLib;
using RimWorld;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), "get_GrowthRate")]
    public class Plant_GrowthPerTick_Postfix
    {
        static void Postfix(Plant __instance, ref float __result)
        {
            if (__instance.Blight is CustomBlight blight)
            {
                __result = blight.AdjustGrowthRate(__result);
            }
        }
    }
}