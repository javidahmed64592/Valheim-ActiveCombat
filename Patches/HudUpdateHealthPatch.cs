using CombatFlow.Food;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>
/// The health bar waits half a second after each rise before it fills, and every rise starts that wait again. With
/// continuous food regen health rises every physics step, so the bar would never move (not even down, when you
/// take damage). Skipping the wait lets the bar follow your health, still filling smoothly.
/// </summary>
[HarmonyPatch(typeof(Hud), nameof(Hud.UpdateHealth))]
internal static class Hud_UpdateHealth_Patch
{
    private static readonly AccessTools.FieldRef<GuiBar, float> DelayTimer =
        AccessTools.FieldRefAccess<GuiBar, float>("m_delayTimer");

    private static void Postfix(Hud __instance)
    {
        if (FoodRegen.Continuous && __instance.m_healthBarFast != null)
        {
            DelayTimer(__instance.m_healthBarFast) = 0f;
        }
    }
}
