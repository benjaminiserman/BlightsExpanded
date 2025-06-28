using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BlightsExpanded.Blights;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlightsExpanded.Patches
{
    [HarmonyPatch(typeof(IncidentWorker_CropBlight), "TryExecuteWorker")]
    public class IncidentWorker_CropBlight_TryExecuteWorker
    {
        public static CustomBlight ChosenBlight;

        private static readonly int ExpectedMatches = 3;

        public static void ChooseBlight()
        {
            ChosenBlight = CustomBlightHelper.PickBlight().CustomBlight;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var getRoom = AccessTools.Method(typeof(RegionAndRoomQuery), nameof(RegionAndRoomQuery.GetRoom));
            var cropBlighted = AccessTools.Method(typeof(Plant), nameof(Plant.CropBlighted));
            var chosenBlight = typeof(IncidentWorker_CropBlight_TryExecuteWorker)
                .GetField(nameof(ChosenBlight), BindingFlags.Public | BindingFlags.Static);
            var taggedStringImplicitCast = typeof(TaggedString)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "op_Implicit"
                            && m.GetParameters()[0].ParameterType == typeof(string));

            var found = 0;
            var originalCodes = new List<CodeInstruction>(instructions);
            var codes = originalCodes.ListFullCopy();

            for (var i = 0; i < codes.Count; i++)
            {
                var instruction = codes[i];
                // pick a random blight after plant.GetRoom()
                if (codes[i].opcode == OpCodes.Call
                    && (MethodInfo)codes[i].operand == getRoom)
                {
                    codes.Insert(i + 2,
                        new CodeInstruction(OpCodes.Call,
                            typeof(IncidentWorker_CropBlight_TryExecuteWorker).GetMethod(nameof(ChooseBlight))));
                    found++;
                }
                // replace blightableNowPlant.CropBlighted with custom blight spawn
                else if (instruction.opcode == OpCodes.Callvirt
                         && (MethodInfo)instruction.operand == cropBlighted)
                {
                    codes.RemoveAt(i);
                    codes.Insert(i - 1,
                        new CodeInstruction(OpCodes.Ldsfld, chosenBlight));
                    codes.Insert(i + 1,
                        new CodeInstruction(OpCodes.Callvirt,
                            typeof(CustomBlight).GetMethod(nameof(CustomBlight.SpawnBlight))));
                    found++;
                }
                // intercept letter
                else if (instruction.opcode == OpCodes.Ldstr
                         && (string)instruction.operand == "LetterLabelCropBlight")
                {
                    while (true)
                    {
                        if (codes[i].opcode == OpCodes.Ldsfld && (FieldInfo)codes[i].operand ==
                            typeof(LetterDefOf).GetRuntimeField(nameof(LetterDefOf.NegativeEvent)))
                        {
                            break;
                        }

                        codes.RemoveAt(i);
                    }

                    codes.InsertRange(i, new CodeInstruction[]
                    {
                        // letterLabel
                        new CodeInstruction(OpCodes.Ldsfld, chosenBlight),
                        new CodeInstruction(OpCodes.Ldfld, typeof(Thing).GetField(nameof(Thing.def))),
                        new CodeInstruction(OpCodes.Castclass, typeof(CustomBlightDef)),
                        new CodeInstruction(OpCodes.Ldfld,
                            typeof(CustomBlightDef).GetField(nameof(CustomBlightDef.letterLabel))),
                        new CodeInstruction(OpCodes.Call, taggedStringImplicitCast),

                        // letterText
                        new CodeInstruction(OpCodes.Ldsfld, chosenBlight),
                        new CodeInstruction(OpCodes.Ldfld, typeof(Thing).GetField(nameof(Thing.def))),
                        new CodeInstruction(OpCodes.Castclass, typeof(CustomBlightDef)),
                        new CodeInstruction(OpCodes.Ldfld,
                            typeof(CustomBlightDef).GetField(nameof(CustomBlightDef.letterText))),
                        new CodeInstruction(OpCodes.Call, taggedStringImplicitCast)
                    });

                    found++;
                }
            }

            if (found != ExpectedMatches)
            {
                Log.Error(
                    $"Found ${found} components of patch {nameof(IncidentWorker_CropBlight_TryExecuteWorker)}, but expected {ExpectedMatches}");
                codes = originalCodes;
            }

            foreach (var instruction in codes)
            {
                yield return instruction;
            }
        }
    }
}