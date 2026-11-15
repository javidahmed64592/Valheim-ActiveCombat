using CombatFlow.Core;

using UnityEngine;

namespace CombatFlow.Regen;

/// <summary>
/// Regen bonuses from skills. They are added to the regen multiplier, the same way status effects such as Rested add
/// theirs, so they stack with those instead of multiplying them.
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
}
