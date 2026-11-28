using ActiveCombat.Core;
using ActiveCombat.Magic;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>
/// ApplyDamage is the last step before health drops, after armour and resistances, and runs on the character's
/// owner, so for the local player it runs on their own client.
/// </summary>
[HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
[Feature(Feature)]
internal static class Character_ApplyDamage_Patch
{
    /// <summary>Without it, every hit is paid from health, as in vanilla.</summary>
    public const string Feature = "Mana shield";

    private static void Prefix(Character __instance, HitData hit)
    {
        if (__instance is Player player && player == Player.m_localPlayer && hit != null)
        {
            ManaShield.Absorb(player, hit);
        }
    }
}
