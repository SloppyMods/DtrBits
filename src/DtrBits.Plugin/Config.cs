using Dalamud.Configuration;

namespace DtrBits.Plugin;

[Serializable]
public sealed class Config : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public bool ShowGearsets = true;
    public bool ShowRepair = true;
    public bool ShowSpiritbond = true;

    public float RepairWarnBelow = 50f;
    public float RepairCriticalBelow = 20f;
    public float BondNearAt = 90f;

    public ushort ColorGood = 45;
    public ushort ColorWarn = 25;
    public ushort ColorCritical = 17;
}
