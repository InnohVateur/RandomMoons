using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;



namespace RandomMoons.ConfigUtils;

public class RMConfig : SyncedConfig2<RMConfig>
{
    //{ get; private set; }
    [field: SyncedEntryField] public SyncedEntry<bool> AutoStart { get; }//

    // Should we explore a new moon once the level has ended ? 
    [field: SyncedEntryField] public SyncedEntry<bool> AutoExplore { get; }

    // Should we be able to visit the same moon twice during a quota ?
    [field: SyncedEntryField] public SyncedEntry<bool> CheckIfVisitedDuringQuota { get;  }

    // Should the player be able to explore multiples times ?
    [field: SyncedEntryField] public SyncedEntry<bool> RestrictedCommandUsage { get; }

    // What kind of moons can be selected ?
    [field: SyncedEntryField] public SyncedEntry<MoonSelection> MoonSelectionType { get; }

    // Variable to try and sync
    [field: SyncedEntryField] public SyncedEntry<int> SyncedVar { get; }

    // Bind config entries
    public RMConfig(ConfigFile cfg) : base("InnohVateur.RandomMoons")
    {
        

        AutoStart = cfg.BindSyncedEntry(
            new ConfigDefinition("General","AutoStart"),
            false,
            new ConfigDescription("Automatically starts the level upon travelling to a random moon")
        );

        AutoExplore = cfg.BindSyncedEntry(
            new ConfigDefinition("General","AutoExplore"),
            false,
            new ConfigDescription("Automatically explore to a random moon upon leaving the level")
        );

        CheckIfVisitedDuringQuota = cfg.BindSyncedEntry(
            new ConfigDefinition("General","RegisterTravels"),
            false,
            new ConfigDescription("The same moon can't be chosen twice while the quota hasn't changed")
        );

        RestrictedCommandUsage = cfg.BindSyncedEntry(
            new ConfigDefinition("General","PreventMultipleTravels"),
            true,
            new ConfigDescription("Prevents the players to execute explore multiple times without landing")
        );

        MoonSelectionType = cfg.BindSyncedEntry(
            new ConfigDefinition("General","MoonSelection"),
            MoonSelection.ALL,
            new ConfigDescription("Can have three values : vanilla, modded or all, to change the moons that can be chosen. (Note : modded input without modded moons would do the same as all)")
        );
            
        SyncedVar = cfg.BindSyncedEntry(
            new ConfigDefinition("Debug leftover", "DebuG leftover"),
            4,
            new ConfigDescription("This is a debug variable, you can ignore it")
        );         

        if (LethalConfigCompatibility.Enabled)
        {
            RandomMoons.Logger.LogInfo("LethalConfig Found");
            //RandomMoons.Logger.LogInfo("Config check:" + AutoStart.Entry);
            LethalConfigCompatibility.Patches(AutoStart.Entry, AutoExplore.Entry, CheckIfVisitedDuringQuota.Entry, RestrictedCommandUsage.Entry, MoonSelectionType.Entry);
        }

        ConfigManager.Register(this);
    }
}

public class LethalConfigCompatibility 
{
    private static bool? _enabled;

    public static bool Enabled
    {
        get
        {
            if (_enabled == null)
            {
                _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig");
            }
            return (bool)_enabled;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void Patches(ConfigEntry<bool> AutoStart, ConfigEntry<bool> AutoExplore, ConfigEntry<bool> CheckIfVisitedDuringQuota, ConfigEntry<bool> RestrictedCommandUsage, ConfigEntry<MoonSelection> MoonSelectionType)
    {
        
        var AutoStart_input = new BoolCheckBoxConfigItem(AutoStart, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });

        var AutoExplore_input = new BoolCheckBoxConfigItem(AutoExplore, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });

        var CheckIfVisitedDuringQuota_input = new BoolCheckBoxConfigItem(CheckIfVisitedDuringQuota, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });

        var RestrictedCommandUsage_input = new BoolCheckBoxConfigItem(RestrictedCommandUsage, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });



        EnumDropDownConfigItem<MoonSelection> moonSelectionType_input = new EnumDropDownConfigItem<MoonSelection>(MoonSelectionType, false);

        LethalConfigManager.AddConfigItem(AutoStart_input);
        LethalConfigManager.AddConfigItem(AutoExplore_input);
        LethalConfigManager.AddConfigItem(CheckIfVisitedDuringQuota_input);
        LethalConfigManager.AddConfigItem(RestrictedCommandUsage_input);
        LethalConfigManager.AddConfigItem(moonSelectionType_input);
        LethalConfigManager.SkipAutoGen();
        
    }
}