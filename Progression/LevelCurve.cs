using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Progression;

/// <summary>
/// Experience needed for each level of the combat skills. Vanilla needs 0.5 × (level + 1)^1.5 + 0.5, so the cost keeps
/// climbing steeply and a whole playthrough tends to end around level 50. A smaller exponent flattens the late levels.
/// </summary>
internal static class LevelCurve
{
    /// <summary>Without it, combat skills need vanilla's experience per level.</summary>
    public const string Feature = "Level curve";

    public static bool TryGetRequirement(Skills.Skill skill, out float requirement)
    {
        requirement = 0f;
        if (skill?.m_info == null || !CombatSkills.Contains(skill.m_info.m_skill))
        {
            return false;
        }

        float level = Mathf.Floor(skill.m_level + 1f);
        requirement = (Mathf.Pow(level, CombatConfig.LevelCurveExponent.Value) * CombatConfig.LevelCurveScale.Value) + 0.5f;
        return true;
    }
}
