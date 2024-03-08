using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAPI.LibTerminal;
using LethalAPI.LibTerminal.Models;
using RandomMoons.Commands;
using RandomMoons.Patches;
using RandomMoons.ConfigUtils;

namespace RandomMoons
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("LethalAPI.Terminal")]
    [BepInDependency("com.sigurd.csync")]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.willis.lc.lethalsettings", BepInDependency.DependencyFlags.SoftDependency)]
    public class RandomMoons : BaseUnityPlugin
    {
        internal const string modGUID = "InnohVateur.RandomMoons";
        internal const string modName = "RandomMoons";
        internal const string modVersion = "1.2.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static RandomMoons Instance;

        private TerminalModRegistry Commands;

        public static new SyncConfig CustomConfig;

        internal static ManualLogSource mls;

        private void Awake()
        {
            CustomConfig = new SyncConfig(base.Config);

            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Loading patches...");
            ApplyPluginPatches();
            mls.LogInfo("Patches loaded !");

            Commands = TerminalRegistry.CreateTerminalRegistry();
            Commands.RegisterFrom(new ExploreCommand());

            mls.LogInfo("RandomMoons is operational !");
            
        }

        private void ApplyPluginPatches()
        {
            harmony.PatchAll(typeof(RandomMoons));
            mls.LogInfo("Patched RandomMoons");

            harmony.PatchAll(typeof(TerminalPatch));
            mls.LogInfo("Patched Terminal");

            harmony.PatchAll(typeof(StartOfRoundPatch));
            mls.LogInfo("Patched StartOfRound");
        }
    }
}
