using ActiveCombat.Core;
using ActiveCombat.Regen;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Replaces the regen delay vanilla sets every time stamina is used.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.RPC_UseStamina))]
[Feature(Feature)]
internal static class Player_RPC_UseStamina_Patch
{
    /// <summary>Without it, stamina waits vanilla's full delay before it regenerates.</summary>
    public const string Feature = "Shorter stamina delay";

    private static void Postfix(Player __instance, float v)
    {
        if (v != 0f)
        {
            __instance.m_staminaRegenTimer = StaminaTiming.RegenDelay(__instance);
        }
    }
}
