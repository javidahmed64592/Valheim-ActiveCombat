using System.Collections.Generic;

using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Progression;

/// <summary>
/// Replaces vanilla's skill loss on death. Vanilla takes a share of every skill's level (5% at the default world
/// setting) and wipes the progress towards the next level, which late in the game costs several levels. Combat skills
/// instead never lose a level: they lose part of a level's worth of progress, stopping at the start of the current
/// level. Other skills lose as in vanilla.
/// </summary>
internal static class DeathPenalty
{
    /// <summary>Without it, dying lowers combat skills as in vanilla.</summary>
    public const string Feature = "Death penalty";

    /// <summary>The world's highest scaled death penalty setting (Hard, "SkillReductionRate 150").</summary>
    private const float HardestRate = 1.5f;

    /// <summary>Lowers every skill on death.</summary>
    /// <param name="skills">The dying player's skills.</param>
    /// <param name="factor">The share of each level vanilla would take, already scaled by the world's death penalty
    /// setting.</param>
    public static void LowerAllSkills(Skills skills, float factor)
    {
        float progressLoss = ProgressLoss(skills, factor);
        foreach (KeyValuePair<Skills.SkillType, Skills.Skill> entry in skills.m_skillData)
        {
            Skills.Skill skill = entry.Value;
            if (CombatSkills.Contains(entry.Key))
            {
                float lost = progressLoss * skill.GetNextLevelRequirement();
                skill.m_accumulator = Mathf.Max(0f, skill.m_accumulator - lost);
            }
            else
            {
                skill.m_level -= skill.m_level * factor;
                skill.m_accumulator = 0f;
            }
        }
    }

    /// <summary>
    /// Share of a level's progress lost: <see cref="CombatConfig.DeathProgressLoss"/> on Hard, scaled down with the
    /// world's death penalty setting (Normal 1, Easy 0.5, Very Easy and Casual 0.15, against Hard's 1.5).
    /// </summary>
    private static float ProgressLoss(Skills skills, float factor)
    {
        if (skills.m_DeathLowerFactor <= 0f)
        {
            return 0f;
        }
        float rate = factor / skills.m_DeathLowerFactor;
        return Mathf.Clamp01(CombatConfig.DeathProgressLoss.Value * rate / HardestRate);
    }
}
