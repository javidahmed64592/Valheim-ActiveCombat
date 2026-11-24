using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using ActiveCombat.Adrenaline;
using ActiveCombat.Core;
using ActiveCombat.Progression;

using HarmonyLib;

using UnityEngine;

namespace ActiveCombat.Patches;

/// <summary>Routes vanilla's hit adrenaline through <see cref="AdrenalineFlow.AddHitAdrenaline"/>.</summary>
internal static class HitAdrenaline
{
    private static readonly MethodInfo Replacement = AccessTools.Method(
        typeof(AdrenalineFlow), nameof(AdrenalineFlow.AddHitAdrenaline));

    public static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> instructions, string patchName)
    {
        return Transpile.ReplaceCall(instructions, nameof(Character.AddAdrenaline), Replacement, patchName);
    }
}

/// <summary>Melee hits, one gain per character hit (the miss penalty in here passes through unchanged).</summary>
[HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))]
internal static class Attack_DoMeleeAttack_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitAdrenaline.Patch(instructions, nameof(Attack_DoMeleeAttack_Patch));
    }
}

/// <summary>Area attacks such as the atgeir spin, one gain per attack.</summary>
[HarmonyPatch(typeof(Attack), nameof(Attack.DoAreaAttack))]
internal static class Attack_DoAreaAttack_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitAdrenaline.Patch(instructions, nameof(Attack_DoAreaAttack_Patch));
    }
}

/// <summary>Arrows, bolts, spears and staff projectiles hitting a character.</summary>
[HarmonyPatch(typeof(Projectile), nameof(Projectile.OnHit))]
internal static class Projectile_OnHit_Patch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return HitAdrenaline.Patch(instructions, nameof(Projectile_OnHit_Patch));
    }
}

/// <summary>Every hit on a character is sent through here, right before vanilla gives that hit's adrenaline and skill experience.</summary>
[HarmonyPatch(typeof(Character), nameof(Character.Damage))]
internal static class Character_Damage_Patch
{
    private static void Prefix(Character __instance, HitData hit)
    {
        if (hit != null)
        {
            HitDamage.OnHit(__instance, hit);
            SkillXp.OnHit(__instance, hit);
        }
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.AddAdrenaline))]
internal static class Player_AddAdrenaline_Patch
{
    private static void Prefix(Player __instance, out float __state)
    {
        __state = __instance.m_adrenaline;
    }

    private static void Postfix(Player __instance, float v, float __state)
    {
        AdrenalineFlow.AfterAdd(__instance, v, __state);
    }
}

/// <summary>Passes vanilla's adrenaline decay rate through <see cref="AdrenalineFlow.ScaleDecay"/>.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateStats), typeof(float))]
internal static class Player_UpdateStats_Patch
{
    private static readonly FieldInfo DegenCurve = AccessTools.Field(typeof(Player), nameof(Player.m_adrenalineDegen));

    private static readonly MethodInfo Evaluate = AccessTools.Method(typeof(AnimationCurve), nameof(AnimationCurve.Evaluate));

    private static readonly MethodInfo Scale = AccessTools.Method(typeof(AdrenalineFlow), nameof(AdrenalineFlow.ScaleDecay));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool afterCurve = false;
        int count = 0;
        foreach (CodeInstruction instruction in instructions)
        {
            yield return instruction;
            if (instruction.LoadsField(DegenCurve))
            {
                afterCurve = true;
            }
            else if (afterCurve && instruction.Calls(Evaluate))
            {
                yield return new CodeInstruction(OpCodes.Call, Scale);
                afterCurve = false;
                count++;
            }
        }

        if (count == 0)
        {
            ActiveCombatPlugin.Log.LogWarning($"{nameof(Player_UpdateStats_Patch)}: found nothing to patch. The game may have changed, so slower adrenaline decay is off.");
        }
    }
}
