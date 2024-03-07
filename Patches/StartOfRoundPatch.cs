using HarmonyLib;
using GameNetcodeStuff;
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
        public static void updateStates()
        {
            if(StartOfRound.Instance.shipHasLanded && States.hasGambled)
            {
                States.hasGambled = false;
                States.visitedMoons.Add(States.lastVisitedMoon);
            }

            if(StartOfRound.Instance.suckingPlayersOutOfShip)
            {
                States.visitedMoons = [];
            }

            if(StartOfRound.Instance.travellingToNewLevel && States.startUponArriving)
            {
                States.confirmedAutostart = true;
                States.startUponArriving = false;
            }
            if(!StartOfRound.Instance.travellingToNewLevel && States.confirmedAutostart)
            {
                States.confirmedAutostart = false;
                StartOfRound.Instance.StartGameServerRpc();
            }
        }
    }
}
