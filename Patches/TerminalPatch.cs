using HarmonyLib;
using System;
using System.Collections.Generic;
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
    }
}
