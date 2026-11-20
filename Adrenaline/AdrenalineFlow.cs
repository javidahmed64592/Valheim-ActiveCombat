using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Adrenaline;

/// <summary>
/// Reworks how the local player's adrenaline fills and drains. Weapon hits give more, more again while you keep
/// hitting within a short window, and more or less depending on how hard the hit landed compared with a normal hit
/// from your weapon. Adrenaline waits longer before draining and drains slower the better you are with your weapon,
/// and a full bar that triggers a trinket drops back to a share instead of empty. Adrenaline lives on the player's
/// own client, so only the local player is handled.
/// </summary>
internal static class AdrenalineFlow
{
    /// <summary>Hits closer together than this are one swing hitting several targets, so they share a combo step.</summary>
    private const float SameSwingSeconds = 0.15f;

    private static int _comboStreak;
    private static float _lastHitTime = float.NegativeInfinity;

    /// <summary>
    /// Stands in for vanilla's adrenaline gain on a weapon or projectile hit. Misses and other losses pass straight
    /// through.
    /// </summary>
    public static void AddHitAdrenaline(Character character, float amount)
    {
        if (amount <= 0f || character != Player.m_localPlayer)
        {
            character.AddAdrenaline(amount);
            return;
        }

        float gain = amount * CombatConfig.AdrenalineHitGain.Value * Combo() * HitDamage.TakeFactor();
        character.AddAdrenaline(gain);
    }

    /// <summary>Steps the combo for a hit now and returns its gain multiplier.</summary>
    private static float Combo()
    {
        float now = Time.time;
        float sinceLast = now - _lastHitTime;
        if (sinceLast > CombatConfig.AdrenalineComboWindow.Value)
        {
            _comboStreak = 0;
        }
        else if (sinceLast > SameSwingSeconds)
        {
            _comboStreak++;
        }

        _lastHitTime = now;
        float multiplier = 1f + (CombatConfig.AdrenalineComboBonus.Value * _comboStreak);
        return Mathf.Min(multiplier, Mathf.Max(1f, CombatConfig.AdrenalineComboMax.Value));
    }

    /// <summary>Vanilla's decay rate, slowed by the player's skill with the weapon in hand.</summary>
    public static float ScaleDecay(float rate)
    {
        Player player = Player.m_localPlayer;
        if (player == null)
        {
            return rate;
        }

        ItemDrop.ItemData weapon = player.GetCurrentWeapon();
        float skill = weapon != null ? player.GetSkillFactor(weapon.m_shared.m_skillType) : 0f;
        return rate * (1f - (CombatConfig.AdrenalineSkillDecayReduction.Value * skill));
    }

    /// <summary>Runs after vanilla adds adrenaline: lengthens the wait before decay and softens the drop on a trinket proc.</summary>
    public static void AfterAdd(Player player, float amount, float before)
    {
        if (amount <= 0f || player != Player.m_localPlayer)
        {
            return;
        }

        float max = player.GetMaxAdrenaline();
        if (max <= 0f)
        {
            return;
        }

        player.m_adrenalineDegenTimer *= CombatConfig.AdrenalineDecayDelayMultiplier.Value;

        // A gain only leaves the bar empty when it filled it, a trinket fired and vanilla emptied it. The extra check
        // skips gains that a world modifier scaled to nothing on an already empty bar.
        if (player.m_adrenaline <= 0f && (before > 0f || amount >= max))
        {
            player.m_adrenaline = max * CombatConfig.AdrenalineProcReset.Value;
        }
    }
}
