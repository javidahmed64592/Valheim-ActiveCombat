using System.Globalization;
using System.Text.RegularExpressions;

using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Magic;

/// <summary>
/// Blood magic staffs (Staff of Protection, Dead Raiser, Spirit Caller, Trollstav and others) cost 40 to 60% of
/// your current health per cast in vanilla, which leaves mages with no defence. The cost is scaled here (to nothing
/// by default), and the item tooltip's "health use" line is changed to match.
/// </summary>
internal static class BloodMagicCost
{
    private static readonly Regex HealthUse = new(
        @"\n\$item_healthuse: <color=orange>(?<value>[0-9]+(?:\.[0-9]+)?)(?<percent>%?)</color>",
        RegexOptions.Compiled);

    /// <summary>The health an attack costs a player, after the multiplier.</summary>
    public static float Scale(float cost)
    {
        return cost * CombatConfig.BloodMagicHealthMultiplier.Value;
    }

    /// <summary>Removes the tooltip's health use line when there's no cost, or scales the number shown.</summary>
    public static string AdjustTooltip(string tooltip)
    {
        float multiplier = CombatConfig.BloodMagicHealthMultiplier.Value;
        return tooltip == null || Mathf.Approximately(multiplier, 1f) || tooltip.IndexOf("$item_healthuse", System.StringComparison.Ordinal) < 0
            ? tooltip
            : HealthUse.Replace(tooltip, match =>
        {
            if (multiplier <= 0f)
            {
                return "";
            }

            float value = float.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture) * multiplier;
            string percent = match.Groups["percent"].Value;
            string shown = percent.Length > 0
                ? value.ToString("0.0", CultureInfo.InvariantCulture)
                : value.ToString("0.##", CultureInfo.InvariantCulture);
            return "\n$item_healthuse: <color=orange>" + shown + percent + "</color>";
        });
    }
}
