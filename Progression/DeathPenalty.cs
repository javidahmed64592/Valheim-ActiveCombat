using System.Collections.Generic;

using ActiveCombat.Core;

namespace ActiveCombat.Progression;

/// <summary>
/// Replaces vanilla's skill loss on death. Vanilla takes a share of every skill's level (5% at the default world
/// setting) and wipes the progress towards the next level. For the combat skills the share is scaled down and the
/// progress can be kept; other skills lose as in vanilla.
/// </summary>
internal static class DeathPenalty
{
    /// <summary>Lowers every skill on death.</summary>
    /// <param name="skills">The dying player's skills.</param>
    /// <param name="factor">The share vanilla would take, already scaled by the world's death penalty setting.</param>
    public static void LowerAllSkills(Skills skills, float factor)
    {
        float combatFactor = factor * CombatConfig.DeathLossMultiplier.Value;
        bool keepProgress = CombatConfig.DeathKeepsProgress.Value;
        foreach (KeyValuePair<Skills.SkillType, Skills.Skill> entry in skills.m_skillData)
        {
            Skills.Skill skill = entry.Value;
            if (CombatSkills.Contains(entry.Key))
            {
                skill.m_level -= skill.m_level * combatFactor;
                if (!keepProgress)
                {
                    skill.m_accumulator = 0f;
                }
            }
            else
            {
                skill.m_level -= skill.m_level * factor;
                skill.m_accumulator = 0f;
            }
        }
    }
}
