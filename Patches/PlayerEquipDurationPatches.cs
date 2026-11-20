using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Actions;
using ActiveCombat.Core;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Scales every read of <c>m_equipDuration</c> where the player decides how long equipping takes.</summary>
internal static class EquipDuration
{
    private static readonly FieldInfo Field = AccessTools.Field(
        typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_equipDuration));

    private static readonly MethodInfo Scale = AccessTools.Method(
        typeof(ActionTiming), nameof(ActionTiming.ScaleEquipDuration));

    public static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> instructions, string patchName)
    {
        return Transpile.ScaleField(instructions, Field, Scale, patchName);
    }
}

/// <summary>Deciding between equipping straight away (duration 0) and queueing an action.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.ToggleEquipped))]
internal static class Player_ToggleEquipped_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return EquipDuration.Patch(instructions, nameof(Player_ToggleEquipped_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.QueueEquipAction))]
internal static class Player_QueueEquipAction_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return EquipDuration.Patch(instructions, nameof(Player_QueueEquipAction_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.QueueUnequipAction))]
internal static class Player_QueueUnequipAction_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return EquipDuration.Patch(instructions, nameof(Player_QueueUnequipAction_Patch));
    }
}
