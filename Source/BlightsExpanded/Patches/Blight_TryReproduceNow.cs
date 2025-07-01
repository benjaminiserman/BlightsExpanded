using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BlightsExpanded.Blights;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Blight), "<TryReproduceNow>b__25_0")]
    public class Blight_TryReproduceNow
    {
        private static readonly int ExpectedMatches = 2;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var cropBlighted = AccessTools.Method(typeof(Plant), nameof(Plant.CropBlighted));
            var found = 0;

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Brfalse_S)
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    found++;
                }
                else if (instruction.opcode == OpCodes.Callvirt
                    && (MethodInfo)instruction.operand == cropBlighted)
                {
                    yield return new CodeInstruction(
                        OpCodes.Callvirt,
                        typeof(CustomBlight).GetMethod(nameof(CustomBlight.SpawnBlight))
                    );
                    found++;
                }
                else
                {
                    yield return instruction;
                }
            }

            if (found != ExpectedMatches)
            {
                Log.Error($"Found ${found} components of patch {nameof(Blight_TryReproduceNow)}, but expected {ExpectedMatches}");
            }
        }
    }
}