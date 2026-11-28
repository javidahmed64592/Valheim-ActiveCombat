using ActiveCombat.Core;
using ActiveCombat.Regen;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Stamina regen during attacks and dodges. UpdateStats(float) only runs for the local player, from Player.FixedUpdate on the owner's client.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.UpdateStats), [typeof(float)])]
[Feature(Feature)]
internal static class Player_UpdateStats_ActionRegen_Patch
{
    /// <summary>Without it, stamina waits for attacks and dodges to end before it regenerates, as in vanilla.</summary>
    public const string Feature = "Regen during actions";

    private static void Postfix(Player __instance, float dt)
    {
        ActionRegen.Apply(__instance, dt);
    }
}
