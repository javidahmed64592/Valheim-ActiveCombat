using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Regen;

/// <summary>
/// Regen bonuses from skills. On their own they are a floor: a skilled player always regenerates at least this fast.
/// Next to Rested or another regen effect they have diminishing returns (see <see cref="Combine"/>), so the two
/// don't simply add up.
/// </summary>
internal static class SkillRegen
{
    private static readonly Skills.SkillType[] AgilitySkills =
    [
        Skills.SkillType.Run,
        Skills.SkillType.Jump,
        Skills.SkillType.Swim,
        Skills.SkillType.Sneak,
        Skills.SkillType.Dodge,
    ];

    /// <summary>Scales with the average agility skill, since these all level up just by moving around.</summary>
    public static float StaminaBonus(Player player)
    {
        float total = 0f;
        foreach (Skills.SkillType skill in AgilitySkills)
        {
            total += player.GetSkillFactor(skill);
        }
        return CombatConfig.AgilityRegenBonus.Value * total / AgilitySkills.Length;
    }

    /// <summary>Scales with the best magic school, so a mage who only uses one school isn't penalised.</summary>
    public static float EitrBonus(Player player)
    {
        float best = Mathf.Max(
            player.GetSkillFactor(Skills.SkillType.ElementalMagic),
            player.GetSkillFactor(Skills.SkillType.BloodMagic));
        return CombatConfig.MagicRegenBonus.Value * best;
    }

    /// <summary>
    /// Combines a regen multiplier from status effects with a skill bonus: the bigger of the two bonuses counts in
    /// full and the smaller at <c>SkillStackShare</c>. Effects that slow regen (below 1) still slow the skilled rate.
    /// </summary>
    public static float Combine(float effectsMultiplier, float skillBonus)
    {
        if (effectsMultiplier < 1f)
        {
            return effectsMultiplier * (1f + skillBonus);
        }

        float effectsBonus = effectsMultiplier - 1f;
        return 1f + Mathf.Max(effectsBonus, skillBonus)
            + (CombatConfig.SkillStackShare.Value * Mathf.Min(effectsBonus, skillBonus));
    }
}
