using DtrBits.Core;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace DtrBits.Plugin;

internal static unsafe class Equipped
{
    public static List<float> ConditionPercents() => Read(GearMath.RepairSlots, GearMath.ConditionPercent, false);
    public static List<float> SpiritbondPercents() => Read(GearMath.BondSlots, GearMath.SpiritbondPercent, true);

    private static List<float> Read(int[] slots, Func<ushort, float> convert, bool bond)
    {
        var list = new List<float>();
        var manager = InventoryManager.Instance();
        if (manager == null) return list;
        var container = manager->GetInventoryContainer(InventoryType.EquippedItems);
        if (container == null) return list;
        foreach (var slot in slots)
        {
            var item = container->GetInventorySlot(slot);
            if (item == null || item->ItemId == 0) continue;
            list.Add(convert(bond ? item->SpiritbondOrCollectability : item->Condition));
        }
        return list;
    }
}
