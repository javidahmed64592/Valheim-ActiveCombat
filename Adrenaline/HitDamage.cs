using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Adrenaline;

/// <summary>
/// Compares how hard a hit from the local player lands with a normal hit from their weapon. The target's resistances,
/// weak spots, backstabs and staggers are worked out here the way the target's owner will apply them, so the gain
/// can be decided straight away on the attacker's client. Vanilla gives the hit's adrenaline right after sending the
/// damage, so the factor from the latest hit is the one that gain uses.
/// </summary>
internal static class HitDamage
{
    /// <summary>Vanilla's skill damage roll is centred on this range: 40% of weapon damage at skill 0, 100% at 100.</summary>
    private const float MinSkillDamage = 0.4f;

    private static float _factor = 1f;

    /// <summary>The latest hit's damage factor, used once.</summary>
    public static float TakeFactor()
    {
        float factor = _factor;
        _factor = 1f;
        return factor;
    }

    /// <summary>Called as the local player's hit is sent to a character.</summary>
    public static void OnHit(Character target, HitData hit)
    {
        _factor = 1f;
        float exponent = CombatConfig.AdrenalineDamageScaling.Value;
        if (exponent <= 0f || hit.m_hitType != HitData.HitType.PlayerHit || hit.GetAttacker() is not Player player
            || player != Player.m_localPlayer || target == player)
        {
            return;
        }

        float normal = NormalDamage(player);
        if (normal <= 0f)
        {
            return;
        }

        float cap = Mathf.Max(1f, CombatConfig.AdrenalineDamageGainCap.Value);
        float ratio = LandedDamage(target, hit) / normal;
        _factor = Mathf.Clamp(Mathf.Pow(ratio, exponent), 1f / cap, cap);
    }

    /// <summary>What a plain hit from the player's weapon does to a target with no resistances.</summary>
    private static float NormalDamage(Player player)
    {
        ItemDrop.ItemData weapon = player.GetCurrentWeapon();
        if (weapon == null)
        {
            return 0f;
        }

        float damage = Combat(weapon.GetDamage());
        ItemDrop.ItemData ammo = player.GetAmmoItem();
        if (!string.IsNullOrEmpty(weapon.m_shared.m_ammoType) && ammo != null)
        {
            damage += Combat(ammo.GetDamage());
        }

        return damage * Mathf.Lerp(MinSkillDamage, 1f, player.GetSkillFactor(weapon.m_shared.m_skillType));
    }

    /// <summary>The hit's damage after the target's resistances, backstab and stagger bonuses.</summary>
    public static float LandedDamage(Character target, HitData hit)
    {
        HitData landed = hit.Clone();
        WeakSpot weakSpot = target.GetWeakSpot(target.FindWeakSpotIndex(hit.m_hitCollider));
        landed.ApplyResistance(target.GetDamageModifiers(weakSpot), out _);
        if (target.m_baseAI != null && !target.m_baseAI.IsAlerted() && hit.m_backstabBonus > 1f)
        {
            landed.ApplyModifier(hit.m_backstabBonus);
        }

        if (target.IsStaggering() && !target.IsPlayer())
        {
            landed.ApplyModifier(2f);
        }

        return Combat(landed.m_damage);
    }

    /// <summary>Damage that hurts characters, leaving out chopping and mining.</summary>
    private static float Combat(HitData.DamageTypes damage)
    {
        return damage.m_damage + damage.m_blunt + damage.m_slash + damage.m_pierce + damage.m_fire + damage.m_frost
            + damage.m_lightning + damage.m_poison + damage.m_spirit;
    }
}
