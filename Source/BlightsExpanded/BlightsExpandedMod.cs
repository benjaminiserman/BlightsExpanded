using System.IO;
using System.Reflection;
using BlightsExpanded.Patches;
using HarmonyLib;
using Verse;

namespace BlightsExpanded
{
    public class BlightsExpandedMod : Mod
    {
        private const string Id = "winggar.blightsexpanded";
        internal static string VersionDir => Path.Combine(ModLister.GetActiveModWithIdentifier(Id).RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public BlightsExpandedMod(ModContentPack content) : base(content)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            if (Prefs.DevMode)
            {
                File.WriteAllText(VersionDir, CurrentVersion);
            }

            var harmony = new Harmony(Id);
            harmony.PatchAll();
            JobDriver_PlantWork_MakeNewToils.Apply(harmony);
        }
    }
}