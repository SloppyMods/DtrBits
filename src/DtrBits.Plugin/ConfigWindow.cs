using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace DtrBits.Plugin;

internal sealed class ConfigWindow(Config config, Action save)
    : Window("DTR Bits settings###DtrBitsConfig", ImGuiWindowFlags.AlwaysAutoResize)
{
    public override void Draw()
    {
        var changed = false;
        changed |= ImGui.Checkbox("Gearset switcher entry", ref config.ShowGearsets);
        changed |= ImGui.Checkbox("Repair entry (lowest condition)", ref config.ShowRepair);
        changed |= ImGui.Checkbox("Spiritbond entry (highest)", ref config.ShowSpiritbond);
        ImGui.Separator();
        changed |= ImGui.SliderFloat("Repair yellow below %", ref config.RepairWarnBelow, 1, 100);
        changed |= ImGui.SliderFloat("Repair red below %", ref config.RepairCriticalBelow, 1, 100);
        changed |= ImGui.SliderFloat("Spiritbond yellow from %", ref config.BondNearAt, 1, 100);
        ImGui.Separator();
        var good = (int)config.ColorGood; var warn = (int)config.ColorWarn; var bad = (int)config.ColorCritical;
        changed |= ImGui.InputInt("Green colour id", ref good);
        changed |= ImGui.InputInt("Yellow colour id", ref warn);
        changed |= ImGui.InputInt("Red colour id", ref bad);
        config.ColorGood = (ushort)Math.Clamp(good, 0, 600);
        config.ColorWarn = (ushort)Math.Clamp(warn, 0, 600);
        config.ColorCritical = (ushort)Math.Clamp(bad, 0, 600);
        ImGui.TextDisabled("Defaults: green 45, yellow 25, red 17. Spiritbond 100% shows green.");
        if (changed) save();
    }
}
