using CombatFlow.Regen;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>Only Player.UpdateStats asks for these, and only on the player's own client.</summary>
[HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
internal static class SEMan_ModifyStaminaRegen_Patch
{
    private static void Postfix(SEMan __instance, ref float staminaMultiplier)
    {
        if (__instance.m_character is Player player)
        {
            staminaMultiplier += SkillRegen.StaminaBonus(player);
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
            eitrMultiplier += SkillRegen.EitrBonus(player);
        }
    }
}
