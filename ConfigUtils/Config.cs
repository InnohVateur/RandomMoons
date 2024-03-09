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
    [DataContract]
    public class SyncConfig : SyncedConfig<SyncConfig>
    {

        // Config entries (using CSync prebuilt entry class)
        [DataMember] internal SyncedEntry<bool> autoStart { get; private set; }
        [DataMember] internal SyncedEntry<bool> autoExplore { get; private set; }
        [DataMember] internal SyncedEntry<bool> checkIfVisitedDuringQuota { get; private set; }
        [DataMember] internal SyncedEntry<bool> restrictedCommandUsage { get; private set; }
        [DataMember] internal SyncedEntry<MoonSelection> moonSelectionType { get; private set; }

        // Debug entries
        [DataMember] internal SyncedEntry<bool> autoReachQuota { get; private set; }
        [DataMember] internal SyncedEntry<bool> hasInfiniteCredits { get; private set; }
        [DataMember] internal SyncedEntry<bool> hasNightVision { get; private set; }
        [DataMember] internal SyncedEntry<bool> isPlayerInvincible { get; private set; }
        [DataMember] internal SyncedEntry<bool> doEnemiesAgroPlayer { get; private set; }

        // Constructor (binds the entries and build mod config page for LethalConfig and LethalSettings)
        public SyncConfig(ConfigFile cfg) : base(RandomMoons.modGUID)
        {
            // Register this config for CSync to bless it with their magic man's magic words
            ConfigManager.Register(this);

            // Entry binding
            autoStart = cfg.BindSyncedEntry(
                    "General",
                    "AutoStart",
                    false,
                    "Automatically starts the level upon travelling to a random moon"
                );

            autoExplore = cfg.BindSyncedEntry(
                    "General",
                    "AutoExplore",
                    false,
                    "Automatically explore to a random moon upon leaving the level"
                );

            checkIfVisitedDuringQuota = cfg.BindSyncedEntry(
                    "General",
                    "RegisterTravels",
                    false,
                    "The same moon can't be chosen twice while the quota hasn't changed"
                );

            restrictedCommandUsage = cfg.BindSyncedEntry(
                    "General",
                    "PreventMultipleTravels",
                    true,
                    "Prevents the players to execute explore multiple times without landing"
                );

            moonSelectionType = cfg.BindSyncedEntry(
                    "General",
                    "MoonSelection",
                    MoonSelection.ALL,
                    "Can have three values : vanilla, modded or all, to change the moons that can be chosen. (Note : modded input without modded moons would do the same as all)"
                );


            // Debug entry binding
            autoReachQuota = cfg.BindSyncedEntry(
                    "General.Debug",
                    "AutoReachQuota",
                    false,
                    "Automatically reaches the quota upon landing on Gordion (The Company Building)"
                );

            hasInfiniteCredits = cfg.BindSyncedEntry(
                    "General.Debug",
                    "InfiniteCredits",
                    false,
                    "Constantly sets the credits amount to integer limit (2,147,483,647)"
                );

            hasNightVision = cfg.BindSyncedEntry(
                    "General.Debug",
                    "NightVision",
                    false,
                    "Grants night vision"
                );

            isPlayerInvincible = cfg.BindSyncedEntry(
                    "General.Debug",
                    "Invincibility",
                    false,
                    "Makes the player immune to damages"
                );

            doEnemiesAgroPlayer = cfg.BindSyncedEntry(
                    "General.Debug",
                    "Invisibility",
                    false,
                    "Makes enemies not target the player"
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
            BoolCheckBoxConfigItem autoStartBox = new BoolCheckBoxConfigItem(autoStart.Entry, false);
            BoolCheckBoxConfigItem autoExploreBox = new BoolCheckBoxConfigItem(autoExplore.Entry, false);
            BoolCheckBoxConfigItem checkIfVisitedDuringQuotaBox = new BoolCheckBoxConfigItem(checkIfVisitedDuringQuota.Entry, false);
            BoolCheckBoxConfigItem restrictedCommandUsageBox = new BoolCheckBoxConfigItem(restrictedCommandUsage.Entry, false);
            EnumDropDownConfigItem<MoonSelection> moonSelectionTypeDropdown = new EnumDropDownConfigItem<MoonSelection>(moonSelectionType.Entry, false);

            BoolCheckBoxConfigItem autoReachQuotaBox = new BoolCheckBoxConfigItem(autoReachQuota.Entry, false);
            BoolCheckBoxConfigItem hasInfiniteCreditsBox = new BoolCheckBoxConfigItem(hasInfiniteCredits.Entry, false);
            BoolCheckBoxConfigItem hasNightVisionBox = new BoolCheckBoxConfigItem(hasNightVision.Entry, false);
            BoolCheckBoxConfigItem isPlayerInvincibleBox = new BoolCheckBoxConfigItem(isPlayerInvincible.Entry, false);
            BoolCheckBoxConfigItem doEnemiesAgroPlayerBox = new BoolCheckBoxConfigItem(doEnemiesAgroPlayer.Entry, false);

            LethalConfigManager.SetModDescription("Allows you to travel to a randomly selected moon, for free !");
            LethalConfigManager.SkipAutoGen();

            LethalConfigManager.AddConfigItem(autoStartBox);
            LethalConfigManager.AddConfigItem(autoExploreBox);
            LethalConfigManager.AddConfigItem(checkIfVisitedDuringQuotaBox);
            LethalConfigManager.AddConfigItem(restrictedCommandUsageBox);
            LethalConfigManager.AddConfigItem(moonSelectionTypeDropdown);

            LethalConfigManager.AddConfigItem(autoReachQuotaBox);
            LethalConfigManager.AddConfigItem(hasInfiniteCreditsBox);
            LethalConfigManager.AddConfigItem(hasNightVisionBox);
            LethalConfigManager.AddConfigItem(isPlayerInvincibleBox);
            LethalConfigManager.AddConfigItem(doEnemiesAgroPlayerBox);
        }

        // Inits LethalSettings' mod page
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void InitLethalSettings()
        {
            ModMenu.RegisterMod(new ModMenu.ModSettingsConfig
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
                            OnInitialize = (self) => self.Value = SyncConfig.Default.autoStart.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.autoStart.Value = value
                        },

                        new ToggleComponent {
                            Text = "AutoExplore",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.autoExplore.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.autoExplore.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "RegisterTravels",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.checkIfVisitedDuringQuota.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.checkIfVisitedDuringQuota.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "PreventMultipleTravels",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.restrictedCommandUsage.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.restrictedCommandUsage.Value = value
                        },

                        new DropdownComponent
                        {
                            Options = [
                                new TMPro.TMP_Dropdown.OptionData("ALL"),
                                new TMPro.TMP_Dropdown.OptionData("MODDED"),
                                new TMPro.TMP_Dropdown.OptionData("VANILLA")
                            ],
                            OnInitialize = (self) => self.Value.text = SyncConfig.Default.moonSelectionType.Value.ToString(),
                            OnValueChanged = (self, value) => SyncConfig.Default.moonSelectionType.Value = (MoonSelection)Enum.Parse(typeof(MoonSelection), value.text)
                        },

                        new ToggleComponent
                        {
                            Text = "AutoReachQuota",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.autoReachQuota.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.autoReachQuota.Value = value
                        },

                        new ToggleComponent 
                        {
                            Text = "InfiniteCredits",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.hasInfiniteCredits.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.hasInfiniteCredits.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "NightVision",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.hasNightVision.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.hasNightVision.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "Invincibility",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.isPlayerInvincible.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.isPlayerInvincible.Value = value
                        },

                        new ToggleComponent
                        {
                            Text = "Invisibility",
                            OnInitialize = (self) => self.Value = SyncConfig.Default.doEnemiesAgroPlayer.Value,
                            OnValueChanged = (self, value) => SyncConfig.Default.doEnemiesAgroPlayer.Value = value
                        }
                    ]
            });
        }
    }
}
