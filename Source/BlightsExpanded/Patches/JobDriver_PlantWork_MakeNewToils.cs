using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), nameof(Plant.TickLong))]
    public class JobDriver_PlantWork_MakeNewToils
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var yieldNow = AccessTools.Method(typeof(Plant), nameof(Plant.YieldNow));
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == yieldNow)
                {
                    // OpCodes.Ldloc_1 already on stack
                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call,
                        typeof(CustomBlightHelper).GetMethod(nameof(CustomBlightHelper.BlightOnHarvest)));

                    // put this back so the original YieldNow runs ok
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    found = true;
                }

                yield return instruction;
            }

            if (!found)
            {
                Log.Error($"Failed to find MethodInfo {yieldNow} in patch {nameof(JobDriver_PlantWork_MakeNewToils)}");
            }
        }
    }
}