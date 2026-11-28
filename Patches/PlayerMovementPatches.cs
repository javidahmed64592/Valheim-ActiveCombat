using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Actions;
using ActiveCombat.Core;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Swaps the <c>ClearActionQueue()</c> call in sprinting, jumping and dodging for one that respects the setting.</summary>
internal static class MovementClear
{
    /// <summary>
    /// Running, jumping and dodging don't cancel equipping or reloading. Without it, they do, as in vanilla.
    /// </summary>
    public const string Feature = "Movement doesn't cancel actions";

    private static readonly MethodInfo Replacement = AccessTools.Method(
        typeof(ActionTiming), nameof(ActionTiming.ClearUnlessMovementKeepsActions));

    public static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> instructions, string patchName)
    {
        return Transpile.ReplaceCall(instructions, nameof(Player.ClearActionQueue), Replacement, patchName);
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.CheckRun))]
[Feature(MovementClear.Feature)]
internal static class Player_CheckRun_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_CheckRun_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.OnJump))]
[Feature(MovementClear.Feature)]
internal static class Player_OnJump_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_OnJump_Patch));
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
[Feature(MovementClear.Feature)]
internal static class Player_UpdateDodge_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MovementClear.Patch(instructions, nameof(Player_UpdateDodge_Patch));
    }
}
