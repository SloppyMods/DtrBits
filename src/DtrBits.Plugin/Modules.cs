using DtrBits.Core;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;

namespace DtrBits.Plugin;

internal sealed class RepairModule(GameActions actions) : DtrModule("DTR Bits Repair")
{
    private float? lowest;

    public override bool IsEnabled(Config config) => config.ShowRepair;
    public override string? Tooltip => lowest is { } v ? $"Lowest gear condition {v:0.#}% - click to open Repair" : null;

    public override SeString? Text(Config config)
    {
        lowest = GearMath.Lowest(Equipped.ConditionPercents());
        if (lowest is not { } value) return null;
        var color = GearMath.ConditionTier(value, config.RepairWarnBelow, config.RepairCriticalBelow) switch
        {
            Tier.Good => config.ColorGood,
            Tier.Warn => config.ColorWarn,
            _ => config.ColorCritical,
        };
        return Colored(GearMath.FormatPercent(value), color);
    }

    public override void OnClick(DtrInteractionEvent e) => actions.Use("Repair");
}

internal sealed class SpiritbondModule(GameActions actions) : DtrModule("DTR Bits Spiritbond")
{
    private float? highest;

    public override bool IsEnabled(Config config) => config.ShowSpiritbond;
    public override string? Tooltip => highest is { } v ? $"Highest spiritbond {v:0.#}% - click to open Materia Extraction" : null;

    public override SeString? Text(Config config)
    {
        highest = GearMath.Highest(Equipped.SpiritbondPercents());
        if (highest is not { } value) return null;
        var color = GearMath.SpiritbondState(value, config.BondNearAt) switch
        {
            BondState.Full => config.ColorGood,
            BondState.Near => config.ColorWarn,
            _ => (ushort)0,
        };
        return Colored(GearMath.FormatPercent(value), color);
    }

    public override void OnClick(DtrInteractionEvent e) => actions.Use("Materia Extraction");
}
