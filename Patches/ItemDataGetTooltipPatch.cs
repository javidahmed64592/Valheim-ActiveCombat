using ActiveCombat.Core;
using ActiveCombat.Food;
using ActiveCombat.Magic;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>
/// Keeps the "health use" line in item tooltips in step with the scaled blood magic cost, and adds food's stamina and
/// eitr regen next to its health regen.
/// </summary>
[HarmonyPatch(
    typeof(ItemDrop.ItemData),
    nameof(ItemDrop.ItemData.GetTooltip),
    [typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float), typeof(int), typeof(bool)])]
[Feature(Feature)]
internal static class ItemData_GetTooltip_Patch
{
    /// <summary>Without it, tooltips are vanilla's. Each line it changes also checks that its own feature is on.</summary>
    public const string Feature = "Item tooltips";

    private static void Postfix(ItemDrop.ItemData item, ref string __result)
    {
        if (SafePatcher.IsOn(BloodMagicCost.Feature))
        {
            __result = BloodMagicCost.AdjustTooltip(__result);
        }

        if (item?.m_shared != null && SafePatcher.IsOn(FoodRegen.Feature))
        {
            __result = FoodTooltip.AddRegen(__result, item.m_shared);
        }
    }
}
