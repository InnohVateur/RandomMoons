using GameNetcodeStuff;
using HarmonyLib;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;

namespace RandomMoons.Patches
{
    // Credits to catmcfish which used the night vision code in his mod : https://github.com/catmcfish/LethalCompanyGameMaster

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePatch(ref PlayerControllerB __instance)
        {
            States.clientID = __instance.playerClientId;
            States.defaultNightVision = __instance.nightVision;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePatch(ref PlayerControllerB __instance)
        {
            if(SyncConfig.Default.hasNightVision.Value)
            {
                __instance.SetNightVisionEnabled(true);
            }

            if(SyncConfig.Default.hasInfiniteStamina.Value)
            {
                __instance.sprintMeter = 1f;
            }

            if(__instance.isSprinting)
            {
                __instance.sprintMultiplier = SyncConfig.Default.speedMultiplier.Value;
            }
        }

        [HarmonyPatch("SetNightVisionEnabled")]
        [HarmonyPostfix]
        public static void UpdateNightVisionPatch(ref PlayerControllerB __instance)
        {
            if(SyncConfig.Default.hasNightVision.Value)
            {
                __instance.nightVision.color = UnityEngine.Color.white;
                __instance.nightVision.range = 200000f;
                __instance.nightVision.intensity = 2000f;
            } else
            {
                __instance.nightVision = States.defaultNightVision;
            }
            __instance.nightVision.enabled = true;
        }

        [HarmonyPatch("DamagePlayer")]
        [HarmonyPrefix]
        public static bool DamagePlayerPatch()
        {
            if(SyncConfig.Default.isPlayerInvincible.Value)
            {
                return false;
            }

            return true;
        }

        [HarmonyPatch("AllowPlayerDeath")]
        [HarmonyPrefix]
        public static bool AllowPlayerDeathPatch()
        {
            if(SyncConfig.Default.isPlayerInvincible.Value)
            {
                return false;
            }
            return true;
        }


    }
}
