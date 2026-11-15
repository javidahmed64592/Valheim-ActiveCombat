using CombatFlow.Core;

namespace CombatFlow.Stamina;

internal static class StaminaTiming
{
    /// <summary>How long stamina waits after being used before it regenerates, from the player's vanilla delay.</summary>
    public static float RegenDelay(Player player)
    {
        return player.m_staminaRegenDelay * CombatConfig.StaminaRegenDelayMultiplier.Value;
    }
}
