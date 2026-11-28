using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Core;
using ActiveCombat.Movement;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>An attack can start once the roll's invulnerability is over, instead of after the whole roll.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.StartAttack))]
[Feature(DodgeRoll.AttackFeature)]
internal static class Humanoid_StartAttack_RollPatch
{
    private static readonly MethodInfo Replacement = AccessTools.Method(typeof(DodgeRoll), nameof(DodgeRoll.BlocksAttack));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ReplaceCall(instructions, nameof(Character.InDodge), Replacement, nameof(Humanoid_StartAttack_RollPatch));
    }
}

/// <summary>Dodge skill adds invulnerability after the roll's own.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateDodge))]
[Feature(DodgeRoll.IframeFeature)]
internal static class Player_UpdateDodge_IframePatch
{
    private static void Prefix(Player __instance)
    {
        DodgeRoll.BeforeUpdate(__instance);
    }

    private static void Postfix(Player __instance)
    {
        DodgeRoll.AfterUpdate(__instance);
    }
}
