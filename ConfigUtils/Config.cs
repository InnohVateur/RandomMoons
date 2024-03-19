using BepInEx.Bootstrap;
using BepInEx.Configuration;
using CSync.Lib;
using CSync.Util;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalSettings.UI;
using LethalSettings.UI.Components;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace RandomMoons.ConfigUtils
{
    /// <summary>
    /// Config for the mod. Use Csync to sync client's configs with host's one
    /// </summary>
    [KnownType(typeof(MoonSelection))]
    [DataContract]
    public class SyncConfig : SyncedConfig<SyncConfig>
    {

        // Config entries (using CSync prebuilt entry class)
        [DataMember] public SyncedEntry<bool> AutoStart { get; private set; }
        [DataMember] public SyncedEntry<bool> AutoExplore { get; private set; }
        [DataMember] public SyncedEntry<bool> CheckIfVisitedDuringQuota { get; private set; }
        [DataMember] public SyncedEntry<bool> RestrictedCommandUsage { get; private set; }
        [DataMember] public SyncedEntry<MoonSelection> MoonSelectionType { get; private set; }

        // Constructor (binds the entries and build mod config page for LethalConfig and LethalSettings)
        public SyncConfig(ConfigFile cfg) : base(RandomMoons.modGUID)
        {
            
            // Register this config for CSync to bless it with their magic man's magic words
            ConfigManager.Register(this);

            // Entry binding
            AutoStart = cfg.BindSyncedEntry(
                    "General",
                    "AutoStart",
                    false,
                    "Automatically starts the level upon travelling to a random moon"
                );

            AutoExplore = cfg.BindSyncedEntry(
                    "General",
                    "AutoExplore",
                    false,
                    "Automatically explore to a random moon upon leaving the level"
                );

            CheckIfVisitedDuringQuota = cfg.BindSyncedEntry(
                    "General",
                    "RegisterTravels",
                    false,
                    "The same moon can't be chosen twice while the quota hasn't changed"
                );

            RestrictedCommandUsage = cfg.BindSyncedEntry(
                    "General",
                    "PreventMultipleTravels",
                    true,
                    "Prevents the players to execute explore multiple times without landing"
                );

            MoonSelectionType = cfg.BindSyncedEntry(
                    "General",
                    "MoonSelection",
                    MoonSelection.ALL,
                    "Can have three values : vanilla, modded or all, to change the moons that can be chosen. (Note : modded input without modded moons would do the same as all)"
                );

            // LethalConfig and LethalSettings initialization
            if (Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig"))
            {
                InitLethalConfig();
            }

            if(Chainloader.PluginInfos.ContainsKey("com.willis.lc.lethalsettings"))
            {
                InitLethalSettings();
            }
        }

        // Inits LethalConfig's mod page
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitLethalConfig()
        {
            BoolCheckBoxConfigItem autoStartBox = new BoolCheckBoxConfigItem(AutoStart.Entry, false);
            BoolCheckBoxConfigItem autoExploreBox = new BoolCheckBoxConfigItem(AutoExplore.Entry, false);
            BoolCheckBoxConfigItem checkIfVisitedDuringQuotaBox = new BoolCheckBoxConfigItem(CheckIfVisitedDuringQuota.Entry, false);
            BoolCheckBoxConfigItem restrictedCommandUsageBox = new BoolCheckBoxConfigItem(RestrictedCommandUsage.Entry, false);
            EnumDropDownConfigItem<MoonSelection> moonSelectionTypeDropdown = new EnumDropDownConfigItem<MoonSelection>(MoonSelectionType.Entry, false);

            LethalConfigManager.SetModDescription("Allows you to travel to a randomly selected moon, for free !");
            LethalConfigManager.SkipAutoGen();

            LethalConfigManager.AddConfigItem(autoStartBox);
            LethalConfigManager.AddConfigItem(autoExploreBox);
            LethalConfigManager.AddConfigItem(checkIfVisitedDuringQuotaBox);
            LethalConfigManager.AddConfigItem(restrictedCommandUsageBox);
            LethalConfigManager.AddConfigItem(moonSelectionTypeDropdown);
        }

        // Inits LethalSettings' mod page
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitLethalSettings() => ModMenu.RegisterMod(new ModMenu.ModSettingsConfig
        {
            Name = RandomMoons.modName,
            Id = RandomMoons.modGUID,
            Version = RandomMoons.modVersion,
            Description = "Allows you to travel to a randomly selected moon, for free !",
            MenuComponents =
                    [
                        new ToggleComponent
                        {
                            Text = "AutoStart",
                            OnInitialize = (self) => self.Value = Default.AutoStart.Value,
                            OnValueChanged = (self, value) => Default.AutoStart.Value = value
                        },

                        new ToggleComponent {
                            Text = "AutoExplore",
                            OnInitialize = (self) => self.Value = Default.AutoExplore.Value,
                            OnValueChanged = (self, value) => Default.AutoExplore.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "RegisterTravels",
                            OnInitialize = (self) => self.Value = Default.CheckIfVisitedDuringQuota.Value,
                            OnValueChanged = (self, value) => Default.CheckIfVisitedDuringQuota.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "PreventMultipleTravels",
                            OnInitialize = (self) => self.Value = Default.RestrictedCommandUsage.Value,
                            OnValueChanged = (self, value) => Default.RestrictedCommandUsage.Value = value
                        },

                        new DropdownComponent
                        {
                            Options = [
                                new TMPro.TMP_Dropdown.OptionData("ALL"),
                                new TMPro.TMP_Dropdown.OptionData("MODDED"),
                                new TMPro.TMP_Dropdown.OptionData("VANILLA")
                            ],
                            OnInitialize = (self) => self.Value.text = Default.MoonSelectionType.Value.ToString(),
                            OnValueChanged = (self, value) => Default.MoonSelectionType.Value = (MoonSelection)Enum.Parse(typeof(MoonSelection), value.text)
                        }
                    ]
        });
    }
}
