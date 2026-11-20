using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Actions;
using ActiveCombat.Core;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>
/// Scales the stamina a reload drains each second. The drain is tiny (1/s on crossbows), but it calls UseStamina every
/// frame, which keeps resetting the regen delay, so vanilla stamina never regenerates while you load.
/// </summary>
[HarmonyPatch(typeof(Player), nameof(Player.QueueReloadAction))]
internal static class Player_QueueReloadAction_Patch
{
    private static readonly FieldInfo Field = AccessTools.Field(
        typeof(Attack), nameof(Attack.m_reloadStaminaDrain));

    private static readonly MethodInfo Scale = AccessTools.Method(
        typeof(ActionTiming), nameof(ActionTiming.ScaleReloadStaminaDrain));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ScaleField(instructions, Field, Scale, nameof(Player_QueueReloadAction_Patch));
    }
}
