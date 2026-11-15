using CombatFlow.Regen;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>Replaces the regen delay vanilla sets every time stamina is used.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.RPC_UseStamina))]
internal static class Player_RPC_UseStamina_Patch
{
    private static void Postfix(Player __instance, float v)
    {
        if (v != 0f)
        {
            __instance.m_staminaRegenTimer = StaminaTiming.RegenDelay(__instance);
        }
    }
}
