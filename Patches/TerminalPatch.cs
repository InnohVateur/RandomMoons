using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomMoons.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        [HarmonyPatch("QuitTerminal")]
        [HarmonyPrefix]
        public static void clearTerminalInteraction(Terminal __instance)
        {
            if(States.isInteracting)
            {
                States.closedUponConfirmation = true;
            }
        }

        [HarmonyPatch("BeginUsingTerminal")]
        [HarmonyPrefix]
        public static void registeringMoons(Terminal __instance)
        {
            if(Config.moonSelectionType.Value == MoonSelection.MODDED)
            {
                foreach(SelectableLevel lvl in __instance.moonsCatalogueList) {
                    if(!States.vanillaMoons.Contains(lvl.sceneName)) { return; }
                }

                Config.moonSelectionType.Value = MoonSelection.ALL;
            }
        }
    }
}
