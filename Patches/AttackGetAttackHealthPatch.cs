using ActiveCombat.Magic;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Every health cost of an attack goes through here: the cost per cast and the low-health check.</summary>
[HarmonyPatch(typeof(Attack), "GetAttackHealth")]
internal static class Attack_GetAttackHealth_Patch
{
    private static void Postfix(Attack __instance, ref float __result)
    {
        if (__result > 0f && __instance.m_character is Player)
        {
            __result = BloodMagicCost.Scale(__result);
        }
    }
}
