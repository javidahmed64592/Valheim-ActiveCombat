using CombatFlow.Magic;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>
/// ApplyDamage is the last step before health drops, after armour and resistances, and runs on the character's
/// owner, so for the local player it runs on their own client.
/// </summary>
[HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
internal static class Character_ApplyDamage_Patch
{
    private static void Prefix(Character __instance, HitData hit)
    {
        if (__instance is Player player && player == Player.m_localPlayer && hit != null)
        {
            ManaShield.Absorb(player, hit);
        }
    }
}
