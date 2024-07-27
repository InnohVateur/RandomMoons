using System.Collections.Generic;

namespace RandomMoons.Utils;

/// <summary>
/// All the important state variables / constant variables
/// </summary>
internal class States
{
    public static bool closedUponConfirmation = false; // If the terminal closed while confirming explore command
    public static bool isInteracting = false; // If the player is currently confirming or denying explore command
    public static bool hasGambled = false; // If someone used explore / if the ship autoExplored the current day
    public static bool startUponArriving = false; // If the level needs to start upon finishing travelling
    public static bool confirmedAutostart = false; // Confirms startUponArriving (actually needed)
    public static bool exploreASAP = false; // If the ship should travel to a random moon upon finishing leaving the current level
    public static List<string> visitedMoons = []; // Lists the moons visited by exploring during the current quota

    //Config stuff
    public static bool ConfigStatus;

    public static readonly string[] vanillaMoons = { 
        "Level1Experimentation", "Level2Assurance", "Level3Vow", "Level4March", "Level5Rend", "Level6Dine", "Level7Offense", "Level8Titan", "Level9Artifice", "Level10Adamance", "Level11Embrion"  
    };

    public static readonly int companyBuildingLevelID = 3; // Gordion (Company Building) level ID
    public static string lastVisitedMoon; // Last visited moon using explore command / ship autoExplore
}