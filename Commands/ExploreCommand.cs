using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Interactions;
using LethalAPI.LibTerminal.Interfaces;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;
using System;
using System.Linq;

namespace RandomMoons.Commands
{
    public class ExploreCommand
    {
        [TerminalCommand("explore"), CommandInfo("Let you travel to a random moon for free !")]
        public ITerminalInteraction exec(Terminal terminal)
        {
            States.isInteracting = true;
            return new TerminalInteraction()
                .WithPrompt("You're going to route to a randomly chosen moon, for free.\nPlease CONFIRM or DENY")
                .WithHandler(onInteraction);
        }

        private string onInteraction(Terminal terminal, string s)
        {
            if (States.closedUponConfirmation)
            {
                States.closedUponConfirmation = false;
                States.isInteracting = false;
                terminal.currentNode = new TerminalNode() { name = s };
                terminal.OnSubmit();
                return null;
            }
            if (s.ToLower() == "c" || s.ToLower() == "confirm")
            {
                if (States.hasGambled && SyncConfig.Instance.restrictedCommandUsageSynced.Value)
                {
                    return "You have already explored. Please land before exploring once again !";
                }
                if (StartOfRound.Instance.shipHasLanded || !StartOfRound.Instance.CanChangeLevels() || StartOfRound.Instance.shipIsLeaving)
                {
                    return "Please wait before travelling to a new moon !";
                }
                SelectableLevel moon = chooseRandomMoon(terminal.moonsCatalogueList);
                if (SyncConfig.Instance.autoStartSynced.Value) { States.startUponArriving = true; }
                StartOfRound.Instance.ChangeLevelServerRpc(moon.levelID, terminal.groupCredits);
                States.lastVisitedMoon = moon.PlanetName;
                States.isInteracting = false;
                States.hasGambled = true;

                return "A moon has been picked : " + moon.PlanetName + " (" + moon.currentWeather.ToString() + "). Enjoy the trip !";
            }
            else if (s.ToLower() == "d" || s.ToLower() == "deny")
            {
                States.isInteracting = false;
                return "Route cancelled.";
            }
            else
            {
                States.isInteracting = false;
                terminal.currentNode = new TerminalNode() { name = s };
                terminal.OnSubmit();
                return null;
            }
        }

        private SelectableLevel chooseRandomMoon(SelectableLevel[] moons)
        {
            Random random = new Random();
            int moonIndex = random.Next(0, moons.Length);
            if (SyncConfig.Instance.moonSelectionTypeSynced.Value == MoonSelection.VANILLA && !isMoonVanilla(moons[moonIndex]) || SyncConfig.Instance.moonSelectionTypeSynced.Value == MoonSelection.MODDED && isMoonVanilla(moons[moonIndex]))
            {
                return chooseRandomMoon(moons);
            }
            if (SyncConfig.Instance.checkIfVisitedDuringQuotaSynced.Value && States.visitedMoons.Contains(moons[moonIndex].PlanetName))
            {
                return chooseRandomMoon(moons);
            }
            if (States.visitedMoons.Count == moons.Length)
            {
                States.visitedMoons = [];
            }
            return moons[moonIndex];
        }

        private bool isMoonVanilla(SelectableLevel moon) { return States.vanillaMoons.Contains(moon.sceneName); }
    }
}
