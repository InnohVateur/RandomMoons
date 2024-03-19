using System.Runtime.Serialization;

namespace RandomMoons.ConfigUtils
{
    /// <summary>
    /// Possible values for the MoonSelection config entry
    /// </summary>
    [DataContract]
    public enum MoonSelection
    {
        [EnumMember] ALL,
        [EnumMember] MODDED,
        [EnumMember] VANILLA
    }
}
