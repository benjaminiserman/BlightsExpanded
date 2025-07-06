using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BlightsExpanded.Blights;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(Plant), nameof(Plant.GetInspectString))]
    public class Plant_GetInspectString
    {
        public static readonly int ExpectedMatches = 2;

        public static string GetBlightNameForPlant(Plant plant)
        {
            if (!plant.Blighted)
            {
                return string.Empty;
            }

            return ((CustomBlight)plant.Blight).BlightDef.label
                .Split()
                .Join(c => c.CapitalizeFirst(), " ");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var blighted = typeof(Plant).GetProperty(nameof(Plant.Blighted)).GetMethod;
            var addString =
                typeof(TaggedString).GetMethod("op_Addition", new[] { typeof(TaggedString), typeof(string) });

            var found = 0;
            var originalCodes = new List<CodeInstruction>(instructions);
            var codes = originalCodes.ListFullCopy();

            for (var i = 0; i < codes.Count; i++)
            {
                var instruction = codes[i];
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == blighted)
                {
                    if (found == 0)
                    {
                        codes.RemoveRange(i - 1, 3);
                        found++;
                    }
                }
                else if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == "Blighted")
                {
                    codes.InsertRange(i + 2, new[]
                    {
                        new CodeInstruction(OpCodes.Ldstr, ": "),
                        new CodeInstruction(OpCodes.Call, addString),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call,
                            typeof(Plant_GetInspectString).GetMethod(nameof(GetBlightNameForPlant))),
                        new CodeInstruction(OpCodes.Call, addString)
                    });
                    found++;
                }
            }

            if (found != ExpectedMatches)
            {
                Log.Error(
                    $"Found ${found} components of patch {nameof(Plant_GetInspectString)}, but expected {ExpectedMatches}");
                codes = originalCodes;
            }

            return codes;
        }
    }
}