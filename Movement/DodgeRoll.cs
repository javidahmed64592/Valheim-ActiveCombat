using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Movement;

/// <summary>
/// The roll's timing. Vanilla's roll is invulnerable for about its first 0.4 seconds, then plays a recovery that lasts
/// about half a second more, during which you can't attack. Here an attack can start a short while after the
/// invulnerability ends, and that wait shrinks with Dodge skill until, at 100, you can attack the moment it ends.
/// </summary>
internal static class DodgeRoll
{
    /// <summary>Without it, you wait for the whole roll to finish before attacking, as in vanilla.</summary>
    public const string Feature = "Attack out of a roll";

    /// <summary>When an attack can start, in game time, once the current roll's invulnerability has ended.</summary>
    private static float _attackFrom;

    /// <summary>Whether the roll was invulnerable after the last update.</summary>
    private static bool _wasInvincible;

    /// <summary>
    /// In place of <c>InDodge()</c> where an attack checks it: the roll only blocks an attack until the wait after its
    /// invulnerability is over.
    /// </summary>
    public static bool BlocksAttack(Character character)
    {
        return character.InDodge()
            && !(character is Player player
                && player == Player.m_localPlayer
                && CombatConfig.AttackOutOfRoll.Value
                && SafePatcher.IsOn(Feature)
                && !player.m_dodgeInvincible
                && !_wasInvincible
                && Time.time >= _attackFrom);
    }

    /// <summary>After vanilla updates the roll: when its invulnerability ends, the wait before an attack starts.</summary>
    public static void AfterUpdate(Player player)
    {
        if (player != Player.m_localPlayer)
        {
            return;
        }

        bool invincible = player.m_dodgeInvincible;
        if (_wasInvincible && !invincible)
        {
            float wait = CombatConfig.RollAttackDelay.Value * (1f - player.GetSkillFactor(Skills.SkillType.Dodge));
            _attackFrom = Time.time + wait;
        }

        _wasInvincible = invincible;
    }
}
