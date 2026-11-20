using ActiveCombat.Actions;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Scales reload time for crossbows, the grappling hook and Dundr, after vanilla's skill bonus.</summary>
[HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetWeaponLoadingTime))]
internal static class ItemData_GetWeaponLoadingTime_Patch
{
    private static void Postfix(ItemDrop.ItemData __instance, ref float __result)
    {
        if (__instance.m_shared.m_attack.m_requiresReload)
        {
            __result = ActionTiming.ScaleReloadTime(__result);
        }
    }
}
