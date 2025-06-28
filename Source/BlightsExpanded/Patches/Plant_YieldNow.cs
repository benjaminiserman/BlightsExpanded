using BlightsExpanded.Blights;
using HarmonyLib;
using RimWorld;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), nameof(Plant.YieldNow))]
    public class Plant_YieldNow
    {
        static void Postfix(Plant __instance, ref int __result)
        {
            if (__instance.Blight is CustomBlight blight)
            {
                __result = blight.AdjustYieldAmount(__result);
            }
        }
    }
}