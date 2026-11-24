using System.Collections.Generic;

using ActiveCombat.Adrenaline;
using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Progression;

/// <summary>
/// Scales the experience combat hits give. Vanilla gives a flat amount per swing or projectile that hits anything,
/// whatever the damage or the enemy. Here every character hit is weighed by how much damage it really did (after the
/// target's resistances, capped at the health it had left) and how tough the target is (stars, bosses). Vanilla's
/// raise right after the hits is then multiplied by those weights: the best target counts in full and every further
/// target in the same swing at <see cref="CombatConfig.XpExtraTargetShare"/>. Raises with no recorded hit, such as
/// chopping trees, stay vanilla.
/// </summary>
internal static class SkillXp
{
    private const float MinDamageFactor = 0.5f;
    private const float MaxDamageFactor = 3f;

    private static readonly List<Hit> Pending = [];
    private static readonly List<float> Weights = [];

    private static float _blockFactor = 1f;
    private static bool _blocking;

    /// <summary>Called as a hit is sent to a character, before vanilla raises the attacker's skill.</summary>
    public static void OnHit(Character target, HitData hit)
    {
        Character attacker = hit.GetAttacker();
        if (attacker == null || attacker == target || !EarnsForLocalPlayer(attacker))
        {
            return;
        }

        float landed = Mathf.Min(HitDamage.LandedDamage(target, hit), target.GetHealth());
        Pending.Add(new Hit
        {
            Attacker = attacker,
            Skill = hit.m_skill,
            Weight = DamageFactor(landed) * Toughness(target),
            Frame = Time.frameCount,
        });
    }

    /// <summary>Replaces vanilla's <c>Character.RaiseSkill</c> call after weapon, projectile and area hits.</summary>
    public static void Raise(Character character, Skills.SkillType skill, float value)
    {
        character.RaiseSkill(skill, value * TakeWeight(character, skill));
    }

    /// <summary>Called as the local player starts blocking a hit.</summary>
    public static void BeginBlock(Character blocker, HitData hit, Character attacker)
    {
        _blocking = blocker == Player.m_localPlayer;
        _blockFactor = _blocking ? DamageFactor(hit.GetTotalBlockableDamage()) * Toughness(attacker) : 1f;
    }

    public static void EndBlock()
    {
        _blocking = false;
        _blockFactor = 1f;
    }

    /// <summary>Replaces vanilla's Blocking raise (1 per block, 2 per parry), scaled by the damage blocked.</summary>
    public static void RaiseBlock(Character character, Skills.SkillType skill, float value)
    {
        character.RaiseSkill(skill, _blocking ? value * _blockFactor : value);
    }

    /// <summary>
    /// The local player, or one of their summons: a tamed creature that raises its owner's skill (blood magic) and
    /// follows the local player.
    /// </summary>
    private static bool EarnsForLocalPlayer(Character attacker)
    {
        Player local = Player.m_localPlayer;
        if (local == null)
        {
            return false;
        }
        if (attacker == local)
        {
            return true;
        }
        if (!attacker.IsTamed() || attacker.GetBaseAI() is not MonsterAI ai || ai.GetFollowTarget() != local.gameObject)
        {
            return false;
        }
        Tameable tameable = attacker.GetComponent<Tameable>();
        return tameable != null && tameable.m_levelUpOwnerSkill != Skills.SkillType.None;
    }

    /// <summary>
    /// Combined weight of this frame's hits by <paramref name="character"/> with <paramref name="skill"/>, or 1 when
    /// there are none. Older hits are dropped, so a hit vanilla gives no experience for never carries over.
    /// </summary>
    private static float TakeWeight(Character character, Skills.SkillType skill)
    {
        int frame = Time.frameCount;
        Weights.Clear();
        for (int i = Pending.Count - 1; i >= 0; i--)
        {
            Hit hit = Pending[i];
            if (hit.Frame != frame)
            {
                Pending.RemoveAt(i);
            }
            else if (hit.Attacker == character && hit.Skill == skill)
            {
                Weights.Add(hit.Weight);
                Pending.RemoveAt(i);
            }
        }

        bool scaled = character is not Player || CombatSkills.Contains(skill);
        if (Weights.Count == 0 || !scaled)
        {
            return 1f;
        }

        Weights.Sort();
        float extraShare = CombatConfig.XpExtraTargetShare.Value;
        float total = Weights[Weights.Count - 1];
        for (int i = 0; i < Weights.Count - 1; i++)
        {
            total += Weights[i] * extraShare;
        }
        return total;
    }

    /// <summary>(damage / reference) to the scaling power, between 0.5 and 3.</summary>
    private static float DamageFactor(float damage)
    {
        float scaling = CombatConfig.XpDamageScaling.Value;
        float reference = CombatConfig.XpDamageReference.Value;
        if (scaling <= 0f || reference <= 0f)
        {
            return 1f;
        }
        float factor = damage > 0f ? Mathf.Pow(damage / reference, scaling) : 0f;
        return Mathf.Clamp(factor, MinDamageFactor, MaxDamageFactor);
    }

    private static float Toughness(Character character)
    {
        if (character == null)
        {
            return 1f;
        }
        float toughness = 1f + (CombatConfig.XpStarBonus.Value * Mathf.Max(0, character.GetLevel() - 1));
        if (character.IsBoss())
        {
            toughness *= CombatConfig.XpBossMultiplier.Value;
        }
        return toughness;
    }

    private struct Hit
    {
        public Character Attacker;
        public Skills.SkillType Skill;
        public float Weight;
        public int Frame;
    }
}
