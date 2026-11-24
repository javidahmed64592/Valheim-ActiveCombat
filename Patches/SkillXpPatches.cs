using System.Collections.Generic;
using System.Reflection;

using ActiveCombat.Core;
using ActiveCombat.Progression;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Swaps vanilla's <c>RaiseSkill</c> after hits for one weighted by the hits' damage and targets.</summary>
internal static class HitXp
{
    private static readonly MethodInfo Replacement = AccessTools.Method(typeof(SkillXp), nameof(SkillXp.Raise));

    public static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> instructions, string patchName)
    {
        return Transpile.ReplaceCall(instructions, nameof(Character.RaiseSkill), Replacement, patchName);
    }
}

/// <summary>Melee swings: one raise per swing, after every target is hit.</summary>
[HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))]
internal static class Attack_DoMeleeAttack_XpPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitXp.Patch(instructions, nameof(Attack_DoMeleeAttack_XpPatch));
    }
}

/// <summary>Area attacks: one raise per attack, after every target is hit.</summary>
[HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))]
internal static class Attack_DoAreaAttack_XpPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitXp.Patch(instructions, nameof(Attack_DoAreaAttack_XpPatch));
    }
}

/// <summary>Arrows, bolts, thrown spears and staff projectiles hitting a character.</summary>
[HarmonyPatch(typeof(Projectile), nameof(Projectile.OnHit))]
internal static class Projectile_OnHit_XpPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitXp.Patch(instructions, nameof(Projectile_OnHit_XpPatch));
    }
}

/// <summary>Explosions such as the fireball: vanilla raises once, for the first target hit.</summary>
[HarmonyPatch(typeof(Aoe), nameof(Aoe.OnHit))]
internal static class Aoe_OnHit_XpPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitXp.Patch(instructions, nameof(Aoe_OnHit_XpPatch));
    }
}

[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
internal static class Humanoid_BlockAttack_XpPatch
{
    private static readonly MethodInfo Replacement = AccessTools.Method(typeof(SkillXp), nameof(SkillXp.RaiseBlock));

    private static void Prefix(Humanoid __instance, HitData hit, Character attacker)
    {
        SkillXp.BeginBlock(__instance, hit, attacker);
    }

    private static void Finalizer()
    {
        SkillXp.EndBlock();
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return Transpile.ReplaceCall(instructions, nameof(Character.RaiseSkill), Replacement, nameof(Humanoid_BlockAttack_XpPatch));
    }
}
