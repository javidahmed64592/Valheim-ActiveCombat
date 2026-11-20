using ActiveCombat.Core;
using ActiveCombat.Regen;

namespace ActiveCombat.Food;

/// <summary>
/// Food restores stamina and eitr as well as health. Each food's stamina and eitr regen keep the same proportion to
/// its stamina and eitr as its health regen has to its health, scaled by the <c>Food.StaminaRegen</c> and
/// <c>Food.EitrRegen</c> settings. With <c>Food.ContinuousRegen</c> all three flow in every frame at the same rate
/// per 10 seconds, instead of arriving in one go on vanilla's 10-second tick. Food stamina pauses whenever vanilla
/// stamina regen is stopped by swimming or being over-encumbered.
/// </summary>
internal static class FoodRegen
{
    /// <summary>Seconds between vanilla's food regen ticks.</summary>
    public const float TickSeconds = 10f;

    /// <summary>Stamina this food restores per tick.</summary>
    public static float Stamina(ItemDrop.ItemData.SharedData food)
    {
        return PerTick(food, food.m_foodStamina) * CombatConfig.FoodStaminaRegen.Value;
    }

    /// <summary>Eitr this food restores per tick.</summary>
    public static float Eitr(ItemDrop.ItemData.SharedData food)
    {
        return PerTick(food, food.m_foodEitr) * CombatConfig.FoodEitrRegen.Value;
    }

    /// <summary>Whether food regen flows in every frame instead of on vanilla's tick.</summary>
    public static bool Continuous => CombatConfig.FoodContinuousRegen.Value;

    /// <summary>
    /// Restores what every eaten food gives over <paramref name="seconds"/>. Health is only included when the regen is
    /// continuous, since vanilla's tick heals otherwise; it uses the same health regen effects (Rested and so on).
    /// </summary>
    public static void Restore(Player player, float seconds)
    {
        float share = seconds / TickSeconds;
        float health = 0f;
        float stamina = 0f;
        float eitr = 0f;
        foreach (Player.Food food in player.m_foods)
        {
            health += food.m_item.m_shared.m_foodRegen;
            stamina += Stamina(food.m_item.m_shared);
            eitr += Eitr(food.m_item.m_shared);
        }

        if (Continuous && health > 0f)
        {
            float multiplier = 1f;
            player.m_seman.ModifyHealthRegen(ref multiplier);
            player.Heal(health * multiplier * share, showText: false);
        }

        stamina *= share;
        eitr *= share;
        // Swimming and being over-encumbered stop food's stamina too, as they stop vanilla stamina regen.
        if (stamina > 0f && !ActionRegen.StaminaRegenBlocked(player))
        {
            player.AddStamina(stamina);
        }
        if (eitr > 0f)
        {
            player.AddEitr(eitr);
        }
    }

    private static float PerTick(ItemDrop.ItemData.SharedData food, float amount)
    {
        return food.m_food > 0f ? food.m_foodRegen * amount / food.m_food : 0f;
    }
}
