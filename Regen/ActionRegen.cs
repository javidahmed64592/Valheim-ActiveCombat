using CombatFlow.Core;

using UnityEngine;

namespace CombatFlow.Regen;

/// <summary>
/// Vanilla stops stamina regen for the whole attack or dodge animation, recovery included, so after a swing stamina
/// only starts coming back once the animation ends, however short the regen delay. This gives back a share of the
/// normal regen during those animations, once the regen delay is over. Swimming, wall running and being
/// over-encumbered still stop regen, as in vanilla.
/// </summary>
internal static class ActionRegen
{
    /// <summary>Whether vanilla stops stamina regen for a reason other than attacking or dodging.</summary>
    public static bool StaminaRegenBlocked(Player player)
    {
        return (player.IsSwimming() && !player.IsOnGround()) || player.m_wallRunning || player.IsEncumbered();
    }

    /// <summary>Runs after Player.UpdateStats and adds the regen vanilla skipped because of an attack or dodge.</summary>
    public static void Apply(Player player, float dt)
    {
        float share = CombatConfig.StaminaRegenWhileActing.Value;
        if (share <= 0f || player.InIntro() || player.IsTeleporting()
            || !(player.InAttack() || player.InDodge()) || StaminaRegenBlocked(player)
            || player.m_staminaRegenTimer > 0f)
        {
            return;
        }

        float maxStamina = player.GetMaxStamina();
        if (maxStamina <= 0f || player.m_stamina >= maxStamina)
        {
            return;
        }

        // The same sum vanilla's UpdateStats uses, which our skill regen patch on ModifyStaminaRegen also applies to.
        float regen = player.m_staminaRegen
            + ((1f - (player.m_stamina / maxStamina)) * player.m_staminaRegen * player.m_staminaRegenTimeMultiplier);
        if (player.IsBlocking())
        {
            regen *= 0.8f;
        }
        float multiplier = 1f;
        player.m_seman.ModifyStaminaRegen(ref multiplier);

        player.m_stamina = Mathf.Min(maxStamina, player.m_stamina + (regen * multiplier * share * dt * Game.m_staminaRegenRate));
        player.m_nview.GetZDO().Set(ZDOVars.s_stamina, player.m_stamina);
    }
}
