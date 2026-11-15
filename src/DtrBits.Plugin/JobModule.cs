using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;

namespace DtrBits.Plugin;

internal sealed class JobModule(IObjectTable objects, Window gearsets) : DtrModule("DTR Bits Gearsets")
{
    public override bool IsEnabled(Config config) => config.ShowGearsets;
    public override string? Tooltip => "Click to switch gearset";

    public override SeString? Text(Config config)
    {
        var player = objects.LocalPlayer;
        return player is null ? null : Colored(player.ClassJob.Value.Abbreviation.ToString(), 0);
    }

    public override void OnClick(DtrInteractionEvent e) => gearsets.Toggle();
}
