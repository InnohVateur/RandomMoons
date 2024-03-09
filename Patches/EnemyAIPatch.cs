using GameNetcodeStuff;
using HarmonyLib;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;
using UnityEngine;

namespace RandomMoons.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void execInvisibility(EnemyAI __instance)
        {
            if(SyncConfig.Default.isPlayerInvisible && __instance.targetPlayer.playerClientId == States.clientID)
            {
                __instance.targetPlayer = null;
            }
        }
    }
}
