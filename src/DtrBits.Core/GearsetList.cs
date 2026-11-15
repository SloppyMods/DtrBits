namespace DtrBits.Core;

public sealed record GearsetInfo(int Id, string Name, string Job, int Level, uint RoleOrder, uint JobOrder);

public static class GearsetList
{
    public static IReadOnlyList<GearsetInfo> Sorted(IEnumerable<GearsetInfo> sets) =>
        sets.OrderBy(s => s.RoleOrder).ThenBy(s => s.JobOrder).ThenBy(s => s.Id).ToList();
}
