using BepInEx.Logging;
using CSync;
using CSync.Lib;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Interactions;
using LethalAPI.LibTerminal.Interfaces;
using RandomMoons.ConfigUtils;
using RandomMoons.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomMoons.Commands
{
    /// <summary>
    /// This command is  used to display the current user's configuration of the mod. This is mainly used to debug, and for conveniance purposes when not using
    /// Lethal config or Lethal settings
    /// </summary>
    /// 
    public class DisplayConfigCommand
    {

        [TerminalCommand("RmConfig"), CommandInfo("Displays RandomMoons config.")]
        public string DisplayConfigString()
        {
            ///<summary>
            ///This function aim's is to return all of the current config in a single string, to be later printed on the terminal 
            ///when the command is called
            ///</summary>

            string ConfigString;
         

            //We want to create a string that contain all of the current config, and the state of the syncing.

            ConfigString = $"{RandomMoons.Config.AutoStart}, {RandomMoons.Config.AutoExplore}, {RandomMoons.Config.CheckIfVisitedDuringQuota}, "
                + $"{RandomMoons.Config.RestrictedCommandUsage}, {RandomMoons.Config.MoonSelectionType}";




            return ConfigString;

        }
    }


}
