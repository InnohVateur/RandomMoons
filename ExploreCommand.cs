using BepInEx.Logging;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Interactions;
using LethalAPI.LibTerminal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomMoons
{
    internal class ExploreCommand
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
            if(States.closedUponConfirmation)
            {
                States.closedUponConfirmation = false;
                terminal.currentNode = new TerminalNode() { name = s };
                terminal.OnSubmit();
                return null;
            }
            if (s.ToLower() == "c" || s.ToLower() == "confirm")
            {
                if(States.hasGambled)
                {
                    return "You have already explored. Please land before exploring once again !";
                }
                if (StartOfRound.Instance.shipHasLanded || !StartOfRound.Instance.CanChangeLevels())
                {
                    return "Please wait before travelling to a new moon !";
                }
                Random random = new Random();
                int moonIndex = random.Next(0, terminal.moonsCatalogueList.Length);
                StartOfRound.Instance.ChangeLevelServerRpc(terminal.moonsCatalogueList[moonIndex].levelID, terminal.groupCredits);
                States.isInteracting = false;
                States.hasGambled = true;

                return "A moon has been picked : " + terminal.moonsCatalogueList[moonIndex].PlanetName + ". Enjoy the trip !";
            }
            else if(s.ToLower() == "d" || s.ToLower() == "deny")
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
    }
}
