using CombatFlow.Falling;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>Adjusts the fall height just before vanilla turns it into fall damage on landing.</summary>
[HarmonyPatch(typeof(Character), nameof(Character.UpdateGroundContact))]
internal static class Character_UpdateGroundContact_Patch
{
    private static void Prefix(Character __instance)
    {
        FallHeight.Update(__instance);
    }
}
