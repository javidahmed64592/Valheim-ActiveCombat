using System.Globalization;
using System.Text.RegularExpressions;

namespace CombatFlow.Food;

/// <summary>Shows food's stamina and eitr regen on the vanilla "hp/tick" line of its tooltip.</summary>
internal static class FoodTooltip
{
    private static readonly Regex HealthRegenLine = new(@"(\$item_food_regen: <color=orange>[^<]*hp/tick</color>)");

    public static string AddRegen(string tooltip, ItemDrop.ItemData.SharedData food)
    {
        float stamina = FoodRegen.Stamina(food);
        float eitr = FoodRegen.Eitr(food);
        if (stamina <= 0f && eitr <= 0f)
        {
            return tooltip;
        }

        string extra = "";
        if (stamina > 0f)
        {
            extra += $", <color=#ffff80ff>{Format(stamina)} stamina/tick</color>";
        }
        if (eitr > 0f)
        {
            extra += $", <color=#9090ffff>{Format(eitr)} eitr/tick</color>";
        }
        return HealthRegenLine.Replace(tooltip, "$1" + extra, 1);
    }

    private static string Format(float value)
    {
        return value.ToString("0.#", CultureInfo.InvariantCulture);
    }
}
