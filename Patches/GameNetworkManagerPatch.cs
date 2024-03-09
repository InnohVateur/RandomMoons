using HarmonyLib;
using RandomMoons.Utils;

namespace RandomMoons.Patches
{
    /// <summary>
    /// Patches GameNetworkManager. Learn to read
    /// </summary>
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch
    {
        // When player disconnects, makes hasGambled false
        [HarmonyPatch("StartDisconnect")]
        [HarmonyPrefix]
        public static void StartDisconnectPatch()
        {
            States.hasGambled = false;
        }
    }
}
