using System.IO;
using System.Reflection;
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

            new Harmony(Id).PatchAll();
        }
    }
}