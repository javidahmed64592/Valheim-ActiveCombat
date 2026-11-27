using ActiveCombat.Core;
using ActiveCombat.Food;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Remembers the maximums before eating, then fills by the difference once the food is applied.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.EatFood))]
[Feature(Feature)]
internal static class Player_EatFood_Patch
{
    /// <summary>Without it, eating doesn't fill your health, stamina and eitr straight away.</summary>
    public const string Feature = "Eating fills bars";

    private static void Prefix(Player __instance, out FoodFill.Maximums __state)
    {
        __state = FoodFill.Maximums.Of(__instance);
    }

    private static void Postfix(Player __instance, bool __result, FoodFill.Maximums __state)
    {
        if (__result)
        {
            FoodFill.Apply(__instance, __state);
        }
    }
}
