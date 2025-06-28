using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), nameof(Plant.CanYieldNow))]
    public class Plant_CanYieldNow
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var blighted = AccessTools.Property(typeof(Plant), "Blighted");
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == blighted)
                {
                    yield return new CodeInstruction(OpCodes.Call,
                        typeof(CustomBlightHelper).GetMethod(nameof(CustomBlightHelper.BlockHarvestForBlight)));
                    found = true;
                }
                else
                {
                    yield return instruction;
                }
            }

            if (!found)
            {
                Log.Error($"Failed to find MethodInfo {blighted} in patch {nameof(Plant_CanYieldNow)}");
            }
        }
    }
}