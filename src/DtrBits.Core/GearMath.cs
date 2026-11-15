namespace DtrBits.Core;

public enum Tier { Good, Warn, Critical }
public enum BondState { Building, Near, Full }

public static class GearMath
{
    public const float ConditionScale = 300f;
    public const float SpiritbondScale = 100f;

    public static readonly int[] RepairSlots = [0, 1, 2, 3, 4, 6, 7];
    public static readonly int[] BondSlots = [0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12];

    public static float ConditionPercent(ushort raw) => raw / ConditionScale;
    public static float SpiritbondPercent(ushort raw) => raw / SpiritbondScale;

    public static float? Lowest(IEnumerable<float> values) => values.Any() ? values.Min() : null;
    public static float? Highest(IEnumerable<float> values) => values.Any() ? values.Max() : null;

    public static Tier ConditionTier(float percent, float warnBelow, float criticalBelow) =>
        percent < criticalBelow ? Tier.Critical : percent < warnBelow ? Tier.Warn : Tier.Good;

    public static BondState SpiritbondState(float percent, float nearAt) =>
        percent >= 100f ? BondState.Full : percent >= nearAt ? BondState.Near : BondState.Building;

    public static string FormatPercent(float percent) => $"{Math.Floor(percent):0}%";
}
