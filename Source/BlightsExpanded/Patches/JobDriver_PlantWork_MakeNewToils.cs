using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    public class JobDriver_PlantWork_MakeNewToils
    {
        public static void Apply(Harmony harmony)
        {
            harmony.Patch(typeof(JobDriver_PlantWork)
                    .Inner("<>c__DisplayClass11_0")
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .First(m => m.Name == "<MakeNewToils>b__1"),
                transpiler: new HarmonyMethod(typeof(JobDriver_PlantWork_MakeNewToils).GetMethod(nameof(Transpiler),
                    BindingFlags.Static | BindingFlags.NonPublic))
            );
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var yieldNow = AccessTools.Method(typeof(Plant), nameof(Plant.YieldNow));
            var found = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == yieldNow)
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