using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAPI.LibTerminal;
using LethalAPI.LibTerminal.Models;
using RandomMoons.Commands;
using RandomMoons.Patches;
using RandomMoons.ConfigUtils;
using System.Runtime.Serialization;

namespace RandomMoons
{
    /// <summary>
    /// Main plugin class
    /// </summary>
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("LethalAPI.Terminal")] // Thunderstore mod id : LethalAPI-LethalAPI_Terminal-1.0.1
    [BepInDependency("com.sigurd.csync")] // Thunderstore mod id : Sigurd-CSync-3.1.1
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)] // Thunderstore mod id : AinaVT-LethalConfig-1.3.4
    [BepInDependency("com.willis.lc.lethalsettings", BepInDependency.DependencyFlags.SoftDependency)] // Thunderstore mod id : willis81808-LethalSettings-1.4.0
    public class RandomMoons : BaseUnityPlugin
    {
        // Basic mod infos
        internal const string modGUID = "InnohVateur.RandomMoons";
        internal const string modName = "RandomMoons";
        internal const string modVersion = "1.2.3";

        // Harmony instance
        private readonly Harmony harmony = new Harmony(modGUID);

        // Plugin instance
        private static RandomMoons Instance;

        // Terminal API Registry Instance
        private TerminalModRegistry Commands;

        // Config Instance
        public static SyncConfig CustomConfig;

        // Log Source Instance
        internal static ManualLogSource mls;

        //Executed at start
        private void Awake()
        {
            
            // Instantiates the config
            CustomConfig = new SyncConfig(base.Config);

            // Instantiates the Plugin
            if (Instance == null)
            {
                Instance = this;
            }

            // Instantiates the Log Source
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            // Loads Patches
            mls.LogInfo("Loading patches...");
            ApplyPluginPatches();
            mls.LogInfo("Patches loaded !");

            // Instantiates the Terminal API Registry
            Commands = TerminalRegistry.CreateTerminalRegistry();

            // Registers the commands
            Commands.RegisterFrom(new ExploreCommand());

            // Plugin loaded !
            mls.LogInfo("RandomMoons is operational !");
            
        }

        // Apply harmony patches
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
