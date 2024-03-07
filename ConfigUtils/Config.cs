using BepInEx.Configuration;
using CSync.Lib;
using CSync.Util;
using GameNetcodeStuff;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using System;
using System.Runtime.Serialization;
using Unity.Collections;
using Unity.Netcode;

namespace RandomMoons.ConfigUtils
{
    [DataContract]
    public class SyncConfig : SyncedConfig<SyncConfig>
    {
        [DataMember] internal SyncedEntry<bool> autoStartSynced { get; private set; }
        [DataMember] internal SyncedEntry<bool> checkIfVisitedDuringQuotaSynced { get; private set; }
        [DataMember] internal SyncedEntry<bool> restrictedCommandUsageSynced { get; private set; }
        [DataMember] internal SyncedEntry<MoonSelection> moonSelectionTypeSynced { get; private set; }

        public SyncConfig(ConfigFile cfg) : base("RandomMoons")
        {
            ConfigManager.Register(this);


            autoStartSynced = cfg.BindSyncedEntry(
                    "General",
                    "AutoStart",
                    false,
                    "Automatically starts the level upon travelling to a random moon"
                );

            checkIfVisitedDuringQuotaSynced = cfg.BindSyncedEntry(
                    "General",
                    "RegisterTravels",
                    false,
                    "The same moon can't be chosen twice while the quota hasn't changed"
                );

            restrictedCommandUsageSynced = cfg.BindSyncedEntry(
                    "General",
                    "PreventMultipleTravels",
                    true,
                    "Prevents the players to execute explore multiple times without landing"
                );

            moonSelectionTypeSynced = cfg.BindSyncedEntry(
                    "General",
                    "MoonSelection",
                    MoonSelection.ALL,
                    "Can have three values : vanilla, modded or all, to change the moons that can be chosen. (Note : modded input without modded moons would do the same as all)"
                );

            BoolCheckBoxConfigItem autoStartBox = new BoolCheckBoxConfigItem(autoStartSynced.Entry, false);
            BoolCheckBoxConfigItem checkIfVisitedDuringQuotaBox = new BoolCheckBoxConfigItem(checkIfVisitedDuringQuotaSynced.Entry, false);
            BoolCheckBoxConfigItem restrictedCommandUsageBox = new BoolCheckBoxConfigItem(restrictedCommandUsageSynced.Entry, false);
            EnumDropDownConfigItem<MoonSelection> moonSelectionTypeDropdown = new EnumDropDownConfigItem<MoonSelection>(moonSelectionTypeSynced.Entry, false);

            LethalConfigManager.SetModDescription("Allows you to travel to a randomly selected moon, for free !");
            LethalConfigManager.SkipAutoGen();

            LethalConfigManager.AddConfigItem(autoStartBox);
            LethalConfigManager.AddConfigItem(checkIfVisitedDuringQuotaBox);
            LethalConfigManager.AddConfigItem(restrictedCommandUsageBox);
            LethalConfigManager.AddConfigItem(moonSelectionTypeDropdown);
        }
    }
}
