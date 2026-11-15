using System.Collections.Generic;
using System.Reflection;

using CombatFlow.Actions;
using CombatFlow.Core;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>Swaps the <c>ClearActionQueue()</c> call in sprinting, jumping and dodging for one that respects the setting.</summary>
internal static class MovementClear
{
    private static readonly MethodInfo Replacement = AccessTools.Method(
        typeof(ActionTiming), nameof(ActionTiming.ClearUnlessMovementKeepsActions));

    public static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> instructions, string patchName)
    {
        return Transpile.ReplaceCall(instructions, nameof(Player.ClearActionQueue), Replacement, patchName);
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.CheckRun))]
internal static class Player_CheckRun_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_CheckRun_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.OnJump))]
internal static class Player_OnJump_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_OnJump_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
internal static class Player_UpdateDodge_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_UpdateDodge_Patch));
    }
}
