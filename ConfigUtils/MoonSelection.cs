using System.Runtime.Serialization;

namespace RandomMoons.ConfigUtils;

/// <summary>
/// Possible values for the MoonSelection config entry
/// </summary>

public enum MoonSelection
{
    [EnumMember] ALL,       //All the moons
    [EnumMember] MODDED,    //Only Moddeds moons
    [EnumMember] VANILLA,   //Only Vanilla moons
}