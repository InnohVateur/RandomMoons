using System;
using System.Linq;
using HarmonyLib;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;

namespace RandomMoons.Patches;

/// <summary>
/// Patches terminal. Learn to read
/// </summary>
[HarmonyPatch(typeof(Terminal))]
public class TerminalPatch
{
    // Checks if a player leaved upon confirming explore command
    [HarmonyPatch("QuitTerminal")]
    [HarmonyPrefix]
    public static void QuitTerminalPatch()
    {
        if (States.isInteracting)
            States.closedUponConfirmation = true;
    }

    // Checks if the moon selection config entry can be set to MODDED
    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPrefix]
    public static void BeginUsingTerminalPatch(Terminal __instance)
    {
        if (RandomMoons.Config.MoonSelectionType.Value != MoonSelection.MODDED)
            return;

        foreach (SelectableLevel lvl in __instance.moonsCatalogueList) {
            if (!States.vanillaMoons.Contains(lvl.sceneName)) 
                return;
        }

        RandomMoons.Config.MoonSelectionType.LocalValue = MoonSelection.ALL;
    }
}