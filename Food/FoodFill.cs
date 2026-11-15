using CombatFlow.Core;

namespace CombatFlow.Food;

/// <summary>Fills health, stamina and eitr by however much eating raised each maximum.</summary>
internal static class FoodFill
{
    public readonly struct Maximums(float health, float stamina, float eitr)
    {
        public readonly float Health = health;
        public readonly float Stamina = stamina;
        public readonly float Eitr = eitr;

        public static Maximums Of(Player player)
        {
            return new Maximums(player.GetMaxHealth(), player.GetMaxStamina(), player.GetMaxEitr());
        }
    }

    public static void Apply(Player player, Maximums before)
    {
        if (!CombatConfig.FoodFillsBars.Value)
        {
            return;
        }

        Maximums after = Maximums.Of(player);
        if (after.Health > before.Health)
        {
            player.Heal(after.Health - before.Health);
        }
        if (after.Stamina > before.Stamina)
        {
            player.AddStamina(after.Stamina - before.Stamina);
        }
        if (after.Eitr > before.Eitr)
        {
            player.AddEitr(after.Eitr - before.Eitr);
        }
    }
}
