using Dalamud.Game;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;

namespace DtrBits.Plugin;

internal sealed class GameActions(IDataManager data, IPluginLog log)
{
    private readonly Dictionary<string, uint> ids = [];

    public unsafe bool Use(string englishName)
    {
        if (!ids.TryGetValue(englishName, out var id))
        {
            id = data.GetExcelSheet<GeneralAction>(ClientLanguage.English)
                .FirstOrDefault(r => r.Name.ToString() == englishName).RowId;
            ids[englishName] = id;
        }
        if (id == 0)
        {
            log.Warning("DTR Bits: general action '{0}' not found", englishName);
            return false;
        }
        var manager = ActionManager.Instance();
        return manager != null && manager->UseAction(ActionType.GeneralAction, id);
    }
}
