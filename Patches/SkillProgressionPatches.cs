using ActiveCombat.Core;
using ActiveCombat.Progression;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Flatter level curve for the combat skills; used both when raising a skill and for its progress bar.</summary>
[HarmonyPatch(typeof(Skills.Skill), nameof(Skills.Skill.GetNextLevelRequirement))]
[Feature(LevelCurve.Feature)]
internal static class Skill_GetNextLevelRequirement_Patch
{
    private static bool Prefix(Skills.Skill __instance, ref float __result)
    {
        return !LevelCurve.TryGetRequirement(__instance, out __result);
    }
}

/// <summary>Gentler skill loss on death for the combat skills. Vanilla only calls this from <c>Skills.OnDeath</c>.</summary>
[HarmonyPatch(typeof(Skills), nameof(Skills.LowerAllSkills))]
[Feature(DeathPenalty.Feature)]
internal static class Skills_LowerAllSkills_Patch
{
    private static bool Prefix(Skills __instance, float factor)
    {
        DeathPenalty.LowerAllSkills(__instance, factor);
        __instance.m_player.Message(MessageHud.MessageType.TopLeft, "$msg_skills_lowered");
        return false;
    }
}
