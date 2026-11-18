using CombatFlow.Core;

using UnityEngine;

namespace CombatFlow.Magic;

/// <summary>
/// Part of every hit a player takes is paid from eitr instead of health, while they have eitr. It's the defence
/// mages lack: they can't wear heavy armour without losing eitr regen. Players without eitr (no eitr food or gear)
/// are unaffected. Using the eitr also pauses its regen, as spending it on a spell does.
/// </summary>
internal static class ManaShield
{
    /// <summary>Takes the shield's share out of the hit, paid for with the player's eitr.</summary>
    public static void Absorb(Player player, HitData hit)
    {
        float share = CombatConfig.ManaShieldShare.Value;
        float eitrPerDamage = CombatConfig.ManaShieldEitrPerDamage.Value;
        float eitr = player.GetEitr();
        float damage = hit.GetTotalDamage();
        if (share <= 0f || eitr <= 0f || damage <= 0f)
        {
            return;
        }

        // UseEitr scales the cost by the world's eitr rate, so the eitr available is divided by it too.
        float eitrCost = eitrPerDamage * Game.m_eitrRate;
        float absorbed = damage * share;
        if (eitrCost > 0f)
        {
            absorbed = Mathf.Min(absorbed, eitr / eitrCost);
        }

        hit.ApplyModifier((damage - absorbed) / damage);
        player.UseEitr(absorbed * eitrPerDamage);
    }
}
