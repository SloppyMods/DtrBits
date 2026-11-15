using DtrBits.Core;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Lumina.Excel.Sheets;

namespace DtrBits.Plugin;

internal static unsafe class Gearsets
{
    private const int MaxGearsets = 100;

    public static IReadOnlyList<GearsetInfo> Read(IDataManager data)
    {
        var list = new List<GearsetInfo>();
        var module = RaptureGearsetModule.Instance();
        if (module == null) return list;
        var jobs = data.GetExcelSheet<ClassJob>();
        for (var id = 0; id < MaxGearsets; id++)
        {
            if (!module->IsValidGearset(id)) continue;
            var entry = module->GetGearset(id);
            if (entry == null) continue;
            var job = jobs.GetRowOrDefault(entry->ClassJob);
            var role = job?.Role ?? 0;
            list.Add(new GearsetInfo(id, entry->NameString, job?.Abbreviation.ToString() ?? "?", 0,
                role == 0 ? 9u : role, job?.UIPriority ?? 0u));
        }
        return GearsetList.Sorted(list);
    }

    public static void Equip(int id)
    {
        var module = RaptureGearsetModule.Instance();
        if (module != null) module->EquipGearset(id);
    }
}
