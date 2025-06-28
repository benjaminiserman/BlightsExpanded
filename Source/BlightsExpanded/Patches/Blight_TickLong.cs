using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Blight), nameof(Blight.TickLong))]
    public class Blight_TickLong
    {
        // disable all calls to CheckHarmPlant within Blight.TickLong
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var checkHarmPlant = AccessTools.Method(typeof(Blight), "CheckHarmPlant");
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == checkHarmPlant)
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }
            }

            if (!found)
            {
                Log.Error($"Failed to find MethodInfo {checkHarmPlant} in patch {nameof(Blight_TickLong)}");
            }
        }
    }
}