using DtrBits.Core;

var failures = 0;
void Check(string name, bool ok) { if (!ok) { failures++; Console.WriteLine("FAIL " + name); } else Console.WriteLine("ok   " + name); }

Check("condition 30000 is 100", GearMath.ConditionPercent(30000) == 100f);
Check("condition 15000 is 50", GearMath.ConditionPercent(15000) == 50f);
Check("spiritbond 10000 is 100", GearMath.SpiritbondPercent(10000) == 100f);
Check("spiritbond 2500 is 25", GearMath.SpiritbondPercent(2500) == 25f);
Check("lowest of empty is null", GearMath.Lowest([]) is null);
Check("highest of empty is null", GearMath.Highest([]) is null);
Check("lowest picks min", GearMath.Lowest([80f, 12f, 99f]) == 12f);
Check("highest picks max", GearMath.Highest([80f, 12f, 99f]) == 99f);
Check("condition good", GearMath.ConditionTier(80, 50, 20) == Tier.Good);
Check("condition warn", GearMath.ConditionTier(40, 50, 20) == Tier.Warn);
Check("condition critical", GearMath.ConditionTier(10, 50, 20) == Tier.Critical);
Check("condition boundary warn", GearMath.ConditionTier(50, 50, 20) == Tier.Good);
Check("condition boundary critical", GearMath.ConditionTier(20, 50, 20) == Tier.Warn);
Check("bond low building", GearMath.SpiritbondState(40, 90) == BondState.Building);
Check("bond near", GearMath.SpiritbondState(95, 90) == BondState.Near);
Check("bond full", GearMath.SpiritbondState(100, 90) == BondState.Full);
Check("format floors", GearMath.FormatPercent(87.9f) == "87%");
Check("format zero", GearMath.FormatPercent(0f) == "0%");
Check("repair slots skip waist and crystal", !GearMath.RepairSlots.Contains(5) && !GearMath.RepairSlots.Contains(13));
Check("bond slots include accessories", GearMath.BondSlots.Contains(8) && GearMath.BondSlots.Contains(12));
var sets = GearsetList.Sorted([
    new GearsetInfo(3, "WAR", "WAR", 100, 1, 2),
    new GearsetInfo(1, "PLD", "PLD", 100, 1, 1),
    new GearsetInfo(2, "WHM", "WHM", 100, 2, 1),
    new GearsetInfo(9, "PLD alt", "PLD", 100, 1, 1)]);
Check("gearsets sort by role then job then id", sets.Select(s => s.Id).SequenceEqual([1, 9, 3, 2]));
Console.WriteLine(failures == 0 ? "ALL PASSED" : $"{failures} FAILED");
return failures;
