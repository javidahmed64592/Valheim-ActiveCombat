using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Progression;

/// <summary>
/// Vanilla gives 0.1 of a raise per dodge (0.025 experience) when the dodge key is pressed, even with no stamina to
/// dodge, so Dodge takes over 100,000 dodges to reach 50. Here the raise comes when the dodge actually starts, and a
/// dodge with an enemy that has spotted you nearby earns <see cref="CombatConfig.DodgeCombatXp"/> times as much.
/// Perfect dodges (a hit landing during the dodge) earn <see cref="CombatConfig.PerfectDodgeXp"/> times vanilla.
/// </summary>
internal static class DodgeXp
{
    private const float VanillaDodgeRaise = 0.1f;
    private const float CombatRange = 15f;

    public static void OnDodgeStarted(Player player)
    {
        float multiplier = NearAlertedEnemy(player) ? CombatConfig.DodgeCombatXp.Value : 1f;
        player.GetSkills().RaiseSkill(Skills.SkillType.Dodge, VanillaDodgeRaise * multiplier);
    }

    /// <summary>Replaces vanilla's perfect dodge raise.</summary>
    public static void RaisePerfect(Skills skills, Skills.SkillType skill, float value)
    {
        skills.RaiseSkill(skill, value * CombatConfig.PerfectDodgeXp.Value);
    }

    private static bool NearAlertedEnemy(Player player)
    {
        Vector3 position = player.transform.position;
        foreach (Character character in Character.GetAllCharacters())
        {
            if (character == player || character.IsDead()
                || (character.transform.position - position).sqrMagnitude > CombatRange * CombatRange)
            {
                continue;
            }
            BaseAI ai = character.GetBaseAI();
            if (ai != null && ai.IsAlerted() && BaseAI.IsEnemy(player, character))
            {
                return true;
            }
        }
        return false;
    }
}
