using HarmonyLib;
using RandomMoons.Commands;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;
using UnityEngine;

namespace RandomMoons.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void updateStates()
        {
            if (StartOfRound.Instance.shipHasLanded && States.hasGambled)
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
                GameObject startLever = GameObject.Find("StartGameLever");
                if(startLever == null) { return; }
                StartMatchLever startMatchLever = startLever.GetComponent<StartMatchLever>();
                if(startMatchLever == null) { return; }
                startMatchLever.PullLever();
                startMatchLever.LeverAnimation();
                startMatchLever.StartGame();
            }
            if(StartOfRound.Instance.CanChangeLevels() && States.exploreASAP)
            {
                States.exploreASAP = false;
                Terminal terminal = Object.FindObjectOfType<Terminal>();
                if (SyncConfig.Instance.autoStart.Value) { States.startUponArriving = true; }
                if (TimeOfDay.Instance.daysUntilDeadline > 0)
                {
                    SelectableLevel moon = ExploreCommand.chooseRandomMoon(terminal.moonsCatalogueList);
                    StartOfRound.Instance.ChangeLevelServerRpc(moon.levelID, terminal.groupCredits);
                    States.lastVisitedMoon = moon.PlanetName;
                    States.hasGambled = true;
                }else { StartOfRound.Instance.ChangeLevelServerRpc(States.companyBuildingLevelID, terminal.groupCredits); }
            }
        }

        [HarmonyPatch("ShipLeave")]
        [HarmonyPostfix]
        public static void execAutoExplore()
        {
            if(SyncConfig.Instance.autoExplore)
            {
                States.exploreASAP = true;
            }
        }
    }
}
