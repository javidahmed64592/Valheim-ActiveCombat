using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Regen;

/// <summary>
/// Vanilla stops stamina regen for the whole attack or dodge animation, recovery included, so after a swing stamina
/// only starts coming back once the animation ends, however short the regen delay. This gives back a share of the
/// normal regen during those animations, once the regen delay is over. The share is earned with skill: the weapon in
/// hand for attacks (Unarmed when punching) and Dodge for dodges, so a beginner gets almost none and skill 100 gets
/// the full <c>RegenWhileActing</c>. Swimming, wall running and being over-encumbered still stop regen, as in vanilla.
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
        if (CombatConfig.StaminaRegenWhileActing.Value <= 0f || player.InIntro() || player.IsTeleporting()
            || !(player.InAttack() || player.InDodge()) || StaminaRegenBlocked(player)
            || player.m_staminaRegenTimer > 0f)
        {
            return;
        }

        float share = Share(player);
        if (share <= 0f)
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

    /// <summary>The share of normal regen kept during the current attack or dodge, grown with the matching skill.</summary>
    private static float Share(Player player)
    {
        float skill;
        if (player.InDodge())
        {
            skill = player.GetSkillFactor(Skills.SkillType.Dodge);
        }
        else
        {
            // GetCurrentWeapon returns the unarmed weapon when your hands are empty, so punching uses Unarmed.
            ItemDrop.ItemData weapon = player.GetCurrentWeapon();
            skill = weapon != null ? player.GetSkillFactor(weapon.m_shared.m_skillType) : 0f;
        }
        return CombatConfig.StaminaRegenWhileActing.Value * skill;
    }
}
