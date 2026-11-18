using CombatFlow.Food;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>
/// Adds food's stamina and eitr regen. With continuous regen, vanilla's 10-second heal is held off and all three are
/// restored a little every frame instead; otherwise stamina and eitr arrive on the same tick vanilla heals on.
/// </summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateFood))]
internal static class Player_UpdateFood_Patch
{
    private static void Prefix(Player __instance, float dt, bool forceUpdate, out bool __state)
    {
        __state = false;
        if (forceUpdate)
        {
            return;
        }

        if (FoodRegen.Continuous)
        {
            __instance.m_foodRegenTimer = 0f;
            __state = true;
        }
        else
        {
            __state = __instance.m_foodRegenTimer + dt >= FoodRegen.TickSeconds;
        }
    }

    private static void Postfix(Player __instance, float dt, bool __state)
    {
        if (__state)
        {
            FoodRegen.Restore(__instance, FoodRegen.Continuous ? dt : FoodRegen.TickSeconds);
        }
    }
}
