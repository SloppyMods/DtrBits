using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;

namespace DtrBits.Plugin;

internal sealed class GearsetWindow : Window
{
    private const int ButtonsPerRow = 4;
    private readonly IDataManager data;
    private readonly IObjectTable objects;

    public GearsetWindow(IDataManager data, IObjectTable objects)
        : base("Switch gearset###DtrBitsGearsets", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse)
    {
        this.data = data;
        this.objects = objects;
    }

    public override void Draw()
    {
        var sets = Gearsets.Read(data);
        if (sets.Count == 0) { ImGui.TextDisabled("No gearsets found."); return; }
        var currentJob = objects.LocalPlayer?.ClassJob.Value.Abbreviation.ToString();
        uint lastRole = sets[0].RoleOrder;
        var column = 0;
        foreach (var set in sets)
        {
            if (set.RoleOrder != lastRole) { ImGui.NewLine(); column = 0; lastRole = set.RoleOrder; }
            if (column > 0) ImGui.SameLine();
            var current = set.Job == currentJob;
            if (current) ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.5f, 0.2f, 1f));
            if (ImGui.Button($"{set.Job}###set{set.Id}", new Vector2(70, 0)))
            {
                Gearsets.Equip(set.Id);
                IsOpen = false;
            }
            if (current) ImGui.PopStyleColor();
            column = (column + 1) % ButtonsPerRow;
        }
    }
}
