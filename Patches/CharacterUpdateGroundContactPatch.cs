using ActiveCombat.Core;
using ActiveCombat.Falling;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Adjusts the fall height just before vanilla turns it into fall damage on landing.</summary>
[HarmonyPatch(typeof(Character), nameof(Character.UpdateGroundContact))]
[Feature(Feature)]
internal static class Character_UpdateGroundContact_Patch
{
    /// <summary>Without it, falls are measured and hurt as in vanilla.</summary>
    public const string Feature = "Falling";

    private static void Prefix(Character __instance)
    {
        FallHeight.Update(__instance);
    }
}
