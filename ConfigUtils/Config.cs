using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;

namespace RandomMoons.ConfigUtils
{
    public class Config
    {
        internal static ConfigEntry<bool> autoStart;
        internal static ConfigEntry<bool> checkIfVisitedDuringQuota;
        internal static ConfigEntry<bool> restrictedCommandUsage;
        internal static ConfigEntry<MoonSelection> moonSelectionType;

        public Config(ConfigFile cfg)
        {
            autoStart = cfg.Bind(
                    "General",
                    "AutoStart",
                    false,
                    "Automatically starts the level upon travelling to a random moon"
                );

            checkIfVisitedDuringQuota = cfg.Bind(
                    "General",
                    "RegisterTravels",
                    false,
                    "The same moon can't be chosen twice while the quota hasn't changed"
                );

            restrictedCommandUsage = cfg.Bind(
                    "General",
                    "PreventMultipleTravels",
                    true,
                    "Prevents the players to execute explore multiple times without landing"
                );

            moonSelectionType = cfg.Bind(
                    "General",
                    "MoonSelection",
                    MoonSelection.ALL,
                    "Can have three values : vanilla, modded or all, to change the moons that can be chosen. (Note : modded input without modded moons would do the same as all)"
                );

            BoolCheckBoxConfigItem autoStartBox = new BoolCheckBoxConfigItem(autoStart, false);
            BoolCheckBoxConfigItem checkIfVisitedDuringQuotaBox = new BoolCheckBoxConfigItem(checkIfVisitedDuringQuota, false);
            BoolCheckBoxConfigItem restrictedCommandUsageBox = new BoolCheckBoxConfigItem(restrictedCommandUsage, false);
            EnumDropDownConfigItem<MoonSelection> moonSelectionTypeDropdown = new EnumDropDownConfigItem<MoonSelection>(moonSelectionType, false);

            LethalConfigManager.SetModDescription("Allows you to travel to a randomly selected moon, for free !");

            LethalConfigManager.AddConfigItem(autoStartBox);
            LethalConfigManager.AddConfigItem(checkIfVisitedDuringQuotaBox);
            LethalConfigManager.AddConfigItem(restrictedCommandUsageBox);
            LethalConfigManager.AddConfigItem(moonSelectionTypeDropdown);
        }
    }
}
