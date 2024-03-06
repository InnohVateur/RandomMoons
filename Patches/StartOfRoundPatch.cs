using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomMoons.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void hasGambledToggler()
        {
            if(StartOfRound.Instance.shipHasLanded && States.hasGambled)
            {
                States.hasGambled = false;
            }
        }
    }
}
