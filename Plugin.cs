﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAPI.LibTerminal;
using LethalAPI.LibTerminal.Models;
using RandomMoons.Patches;
using UnityEngine;

namespace RandomMoons
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("LethalAPI.Terminal")]
    public class RandomMoons : BaseUnityPlugin
    {
        private const string modGUID = "InnohVateur.RandomMoons";
        private const string modName = "RandomMoons";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static RandomMoons Instance;

        private TerminalModRegistry Commands;

        internal ManualLogSource mls;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Loading patches...");
            ApplyPluginPatches();
            mls.LogInfo("Patches loaded !");

            Commands = TerminalRegistry.CreateTerminalRegistry();
            Commands.RegisterFrom(new ExploreCommand());

            mls.LogInfo("RandomMoons is operational !");
            
        }

        private void ApplyPluginPatches()
        {
            harmony.PatchAll(typeof(RandomMoons));
            mls.LogInfo("Patched RandomMoons");

            harmony.PatchAll(typeof(TerminalPatch));
            mls.LogInfo("Patched Terminal");

            harmony.PatchAll(typeof(StartOfRoundPatch));
            mls.LogInfo("Patched StartOfRound");
        }
    }
}
