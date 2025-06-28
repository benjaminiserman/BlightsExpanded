using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), "get_GrowthRate")]
    public class Plant_GrowthRate_Transpiler
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var blighted = typeof(Plant).GetProperty(nameof(Plant.Blighted)).GetMethod;
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == blighted)
                {
                    yield return new CodeInstruction(OpCodes.Call,
                        typeof(CustomBlightHelper).GetMethod(nameof(CustomBlightHelper.BlockGrowthForBlight)));
                    found = true;
                }
                else
                {
                    yield return instruction;
                }
            }

            if (!found)
            {
                Log.Error($"Failed to find MethodInfo {blighted} in patch {nameof(Plant_GrowthRate_Transpiler)}");
            }
        }
    }
}