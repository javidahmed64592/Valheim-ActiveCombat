using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Core;
using ActiveCombat.Movement;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>An attack can start once the roll's invulnerability is over, instead of after the whole roll.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
[Feature(DodgeRoll.Feature)]
internal static class Humanoid_StartAttack_RollPatch
{
    private static readonly MethodInfo Replacement = AccessTools.Method(typeof(DodgeRoll), nameof(DodgeRoll.BlocksAttack));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ReplaceCall(instructions, nameof(Character.InDodge), Replacement, nameof(Humanoid_StartAttack_RollPatch));
    }
}

/// <summary>Notes when the roll's invulnerability ends, to time the wait before an attack.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
[Feature(DodgeRoll.Feature)]
internal static class Player_UpdateDodge_RollPatch
{
    private static void Postfix(Player __instance)
    {
        DodgeRoll.AfterUpdate(__instance);
    }
}
