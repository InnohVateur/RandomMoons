using System;
using BepInEx;
using BepInEx.Logging;
using CSync;
using CSync.Lib;
using HarmonyLib;
using LethalAPI.LibTerminal;
using LethalAPI.LibTerminal.Models;
using RandomMoons.Commands;
using RandomMoons.ConfigUtils;
using RandomMoons.Patches;
using RandomMoons.Utils;

namespace RandomMoons;

/// <summary>
/// Main plugin class
/// </summary>
[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("LethalAPI.Terminal")] // TODO: update version Thunderstore mod id : LethalAPI-LethalAPI_Terminal-1.0.1
[BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)] // TODO: update version Thunderstore mod id : AinaVT-LethalConfig-1.3.4
[BepInDependency("com.sigurd.csync", "5.0.1")]
public class RandomMoons : BaseUnityPlugin
{
    // Basic mod infos
    internal const string modGUID = "InnohVateur.RandomMoons";
    internal const string modName = "RandomMoons";
    internal const string modVersion = "1.4.0";

    // Harmony instance
    readonly Harmony harmony = new(modGUID);

    // Terminal API Registry Instance
    TerminalModRegistry Commands;

    internal static new ManualLogSource Logger = null!;
    internal static new RMConfig Config { get; private set; } = null!;

    // Executed at start
    void Awake()
    {
        Logger = base.Logger;

        Config = new RMConfig(base.Config);

        //Attempt to fix the check synced
        //Config.InitialSyncCompleted += CheckSynced; //doesnt work       
        //RMConfig RandomMoons.Config.InitialSyncCompleted += (sender, args) => ;
            //Config.InitialSyncComplete += CheckSynced;


            // Create Terminal and register our commands.
            Commands = TerminalRegistry.CreateTerminalRegistry();
        Commands.RegisterFrom(new ExploreCommand());
        Commands.RegisterFrom(new DisplayConfigCommand());


        try {
            Logger.LogInfo("Applying patches...");
            ApplyPluginPatches();

            Logger.LogInfo("Patches applied!");
        } catch(Exception e) {
            Logger.LogError(e);
        }

        // Plugin loaded!
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded and operational. Have fun!");
    }

    // Apply harmony patches
    void ApplyPluginPatches()
    {
        harmony.PatchAll(typeof(RandomMoons));
        Logger.LogInfo("Patched RandomMoons");

        harmony.PatchAll(typeof(TerminalPatch));
        Logger.LogInfo("Patched Terminal");

        harmony.PatchAll(typeof(StartOfRoundPatch));
        Logger.LogInfo("Patched StartOfRound");
    }

    //void CheckSynced(object sender, EventArgs success)
    void CheckSynced(bool sender, bool success) {
        
    /*
        if (!success) {
            Logger.LogWarning("SYNC FAILED");
            States.ConfigStatus = false;
            return;

        }
        if (success)
        {
            Logger.LogInfo("Sync Succesfull ! Have fun !");
            States.ConfigStatus = true;
            return;
        }
     */   
        Logger.LogDebug($"Commands registered! SyncedVar: {RandomMoons.Config.SyncedVar}");
        Logger.LogDebug("success value: " + success);
    }
    
}