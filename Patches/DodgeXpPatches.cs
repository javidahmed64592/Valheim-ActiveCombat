using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Core;
using ActiveCombat.Progression;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Pressing dodge no longer raises the skill; the dodge starting does.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.Dodge))]
[Feature(DodgeXp.Feature)]
internal static class Player_Dodge_XpPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.RemoveCall(instructions, nameof(Skills.RaiseSkill), 3, nameof(Player_Dodge_XpPatch));
    }
}

/// <summary>
/// A queued dodge that starts sets the queue timer to exactly 0; one that waits or lacks stamina only counts it down.
/// </summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
[Feature(DodgeXp.Feature)]
internal static class Player_UpdateDodge_XpPatch
{
    private static void Prefix(Player __instance, float dt, out bool __state)
    {
        __state = __instance.m_queuedDodgeTimer > dt;
    }

    private static void Postfix(Player __instance, bool __state)
    {
        if (__state && __instance.m_queuedDodgeTimer == 0f && __instance == Player.m_localPlayer)
        {
            DodgeXp.OnDodgeStarted(__instance);
        }
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.RPC_HitWhileDodging))]
[Feature(DodgeXp.Feature)]
internal static class Player_RPC_HitWhileDodging_XpPatch
{
    private static readonly MethodInfo Replacement = AccessTools.Method(typeof(DodgeXp), nameof(DodgeXp.RaisePerfect));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ReplaceCall(instructions, nameof(Skills.RaiseSkill), Replacement, nameof(Player_RPC_HitWhileDodging_XpPatch));
    }
}
