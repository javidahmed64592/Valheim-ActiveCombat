using CombatFlow.Food;
using CombatFlow.Magic;

using HarmonyLib;

namespace CombatFlow.Patches;

/// <summary>
/// Keeps the "health use" line in item tooltips in step with the scaled blood magic cost, and adds food's stamina and
/// eitr regen next to its health regen.
/// </summary>
[HarmonyPatch(
    typeof(ItemDrop.ItemData),
    nameof(ItemDrop.ItemData.GetTooltip),
    [typeof(ItemDrop.ItemData), typeof(int), typeof(bool), typeof(float), typeof(int), typeof(bool)])]
internal static class ItemData_GetTooltip_Patch
{
    private static void Postfix(ItemDrop.ItemData item, ref string __result)
    {
        __result = BloodMagicCost.AdjustTooltip(__result);
        if (item?.m_shared != null)
        {
            __result = FoodTooltip.AddRegen(__result, item.m_shared);
        }
    }
}
