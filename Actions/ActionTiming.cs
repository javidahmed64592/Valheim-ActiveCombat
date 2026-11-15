using CombatFlow.Core;

namespace CombatFlow.Actions;

/// <summary>
/// Equipping, unequipping and reloading all run through the player's action queue. Vanilla wipes that queue whenever
/// you sprint, jump or dodge, and takes its timings straight from the item. These hooks are called from the patches
/// in place of those reads and calls.
/// </summary>
internal static class ActionTiming
{
    /// <summary>Called instead of <c>ClearActionQueue()</c> when the player sprints, jumps or dodges.</summary>
    public static void ClearUnlessMovementKeepsActions(Player player)
    {
        if (!CombatConfig.MovementKeepsActions.Value)
        {
            player.ClearActionQueue();
        }
    }

    /// <summary>Scales an item's equip duration. A result of 0 takes vanilla's instant-equip path.</summary>
    public static float ScaleEquipDuration(float duration)
    {
        return duration * CombatConfig.EquipTimeMultiplier.Value;
    }

    public static float ScaleReloadTime(float time)
    {
        return time * CombatConfig.ReloadTimeMultiplier.Value;
    }

    public static float ScaleReloadStaminaDrain(float drain)
    {
        return drain * CombatConfig.ReloadStaminaMultiplier.Value;
    }
}
