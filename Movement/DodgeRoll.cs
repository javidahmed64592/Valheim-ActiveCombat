using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Movement;

/// <summary>
/// The roll's timing. Vanilla's roll is invulnerable for about its first 0.4 seconds, then plays a recovery that lasts
/// about half a second more, during which you can't attack. Here an attack can start as soon as the invulnerability
/// ends, and Dodge skill adds invulnerability after the roll's own, which carries on into that attack.
/// </summary>
internal static class DodgeRoll
{
    /// <summary>Without it, you wait for the whole roll to finish before attacking, as in vanilla.</summary>
    public const string AttackFeature = "Attack out of a roll";

    /// <summary>Without it, the roll is invulnerable for as long as in vanilla, whatever the Dodge skill.</summary>
    public const string IframeFeature = "Dodge skill lengthens i-frames";

    /// <summary>Until when the skill's extra invulnerability lasts, in game time.</summary>
    private static float _bonusUntil;

    /// <summary>Whether vanilla's own roll invulnerability was on after the last update.</summary>
    private static bool _vanillaInvincible;

    /// <summary>
    /// In place of <c>InDodge()</c> where an attack checks it: the roll only blocks an attack while it is still
    /// invulnerable.
    /// </summary>
    public static bool BlocksAttack(Character character)
    {
        return character.InDodge()
            && !(character is Player player
                && player == Player.m_localPlayer
                && CombatConfig.AttackOutOfRoll.Value
                && SafePatcher.IsOn(AttackFeature)
                && !player.m_dodgeInvincible);
    }

    /// <summary>
    /// Before vanilla updates the roll: it compares the invulnerability with its own last value, not with the extra
    /// time, so it only writes when its own changes.
    /// </summary>
    public static void BeforeUpdate(Player player)
    {
        if (player == Player.m_localPlayer)
        {
            player.m_dodgeInvincibleCached = _vanillaInvincible;
        }
    }

    /// <summary>
    /// After vanilla updates the roll: when its invulnerability ends, the skill's extra time starts. While it lasts
    /// the player counts as invulnerable here and, through the shared flag, on other players' machines.
    /// </summary>
    public static void AfterUpdate(Player player)
    {
        if (player != Player.m_localPlayer || !player.m_nview.IsValid())
        {
            return;
        }

        bool vanilla = player.m_dodgeInvincibleCached;
        if (_vanillaInvincible && !vanilla)
        {
            float bonus = SafePatcher.IsOn(IframeFeature)
                ? CombatConfig.RollIframeBonus.Value * player.GetSkillFactor(Skills.SkillType.Dodge)
                : 0f;
            _bonusUntil = Time.time + bonus;
        }

        _vanillaInvincible = vanilla;
        bool invincible = vanilla || (Time.time < _bonusUntil && !player.IsDead());
        player.m_dodgeInvincibleCached = invincible;

        ZDO zdo = player.m_nview.GetZDO();
        if (zdo.GetBool(ZDOVars.s_dodgeinv) != invincible)
        {
            zdo.Set(ZDOVars.s_dodgeinv, invincible);
        }
    }
}
