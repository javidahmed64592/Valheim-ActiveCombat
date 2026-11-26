using ActiveCombat.Actions;

using HarmonyLib;

using UnityEngine;

namespace ActiveCombat.Patches;

/// <summary>Sheathing your weapons, e.g. with the hide key.</summary>
[HarmonyPatch(typeof(Humanoid), nameof(Humanoid.HideHandItems))]
internal static class Humanoid_HideHandItems_Patch
{
    private static void Postfix(Humanoid __instance, bool animation, bool __result)
    {
        if (__result && animation)
        {
            SheatheTiming.Started(__instance);
        }
    }
}

/// <summary>Drawing sheathed weapons. Vanilla only animates when something was sheathed.</summary>
[HarmonyPatch(typeof(Humanoid), "ShowHandItems")]
internal static class Humanoid_ShowHandItems_Patch
{
    private static void Prefix(ItemDrop.ItemData ___m_hiddenLeftItem, ItemDrop.ItemData ___m_hiddenRightItem, out bool __state)
    {
        __state = ___m_hiddenLeftItem != null || ___m_hiddenRightItem != null;
    }

    private static void Postfix(Humanoid __instance, bool animation, bool __state)
    {
        if (__state && animation)
        {
            SheatheTiming.Started(__instance);
        }
    }
}

/// <summary>Minor actions block attacking.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.InMinorAction))]
internal static class Player_InMinorAction_Patch
{
    private static void Postfix(Player __instance, Animator ___m_animator, ref bool __result)
    {
        if (__result && SheatheTiming.EndedEarly(__instance, ___m_animator))
        {
            __result = false;
        }
    }
}

/// <summary>Minor actions keep you walking.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.InMinorActionSlowdown))]
internal static class Player_InMinorActionSlowdown_Patch
{
    private static void Postfix(Player __instance, Animator ___m_animator, ref bool __result)
    {
        if (__result && SheatheTiming.EndedEarly(__instance, ___m_animator))
        {
            __result = false;
        }
    }
}
