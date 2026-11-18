using CombatFlow.Regen;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>
/// Only Player.UpdateStats asks for these, and only on the player's own client. Gear eitr regen is added by vanilla
/// afterwards, so it still counts in full.
/// </summary>
[HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
internal static class SEMan_ModifyStaminaRegen_Patch
{
    private static void Postfix(SEMan __instance, ref float staminaMultiplier)
    {
        if (__instance.m_character is Player player)
        {
            staminaMultiplier = SkillRegen.Combine(staminaMultiplier, SkillRegen.StaminaBonus(player));
        }
    }
}

[HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyEitrRegen))]
internal static class SEMan_ModifyEitrRegen_Patch
{
    private static void Postfix(SEMan __instance, ref float eitrMultiplier)
    {
        if (__instance.m_character is Player player)
        {
            eitrMultiplier = SkillRegen.Combine(eitrMultiplier, SkillRegen.EitrBonus(player));
        }
    }
}
