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
[Feature(Feature)]
internal static class Player_QueueReloadAction_Patch
{
    /// <summary>Without it, loading a crossbow drains stamina and holds off its regen, as in vanilla.</summary>
    public const string Feature = "Loading uses no stamina";

    private static readonly FieldInfo Field = AccessTools.Field(
        typeof(Attack), nameof(Attack.m_reloadStaminaDrain));

    private static readonly MethodInfo Scale = AccessTools.Method(
        typeof(ActionTiming), nameof(ActionTiming.ScaleReloadStaminaDrain));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ScaleField(instructions, Field, Scale, nameof(Player_QueueReloadAction_Patch));
    }
}
