using System.Collections.Generic;

namespace RandomMoons.Utils
{
    internal class States
    {
        public static bool closedUponConfirmation = false;
        public static bool isInteracting = false;
        public static bool hasGambled = false;
        public static bool startUponArriving = false;
        public static bool confirmedAutostart = false;
        public static List<string> visitedMoons = new List<string>();
        public static string[] vanillaMoons = { "Level1Experimentation", "Level2Assurance", "Level3Vow", "Level4March", "Level5Rend", "Level6Dine", "Level7Offense", "Level8Titan" };
        public static string lastVisitedMoon;
    }
}
