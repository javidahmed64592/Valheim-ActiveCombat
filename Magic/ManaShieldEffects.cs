using CombatFlow.Core;

using UnityEngine;

namespace CombatFlow.Magic;

/// <summary>
/// Shows the mana shield at work with the Staff of Protection's own effects: its hit shimmer where the shield takes a
/// hit, and its shatter burst when a hit drains the last of your eitr. Both are networked effects, so other players see
/// them too. Damage over time (burning, poison) hits every second, so the shimmer plays at most every
/// <see cref="HitCooldown"/> seconds; the burst always plays.
/// </summary>
internal static class ManaShieldEffects
{
    private const string HitPrefab = "fx_StaffShield_Hit";
    private const string BreakPrefab = "fx_StaffShield_Break";
    private const float HitCooldown = 0.5f;

    private static float _lastHitTime = float.NegativeInfinity;

    public static void Play(Player player, HitData hit, bool drained)
    {
        if (!CombatConfig.ManaShieldEffects.Value || ZNetScene.instance == null)
        {
            return;
        }

        if (drained)
        {
            // Placed like SE_Shield places it when the staff's shield breaks.
            Spawn(BreakPrefab, player.GetCenterPoint(), player.transform.rotation, player.GetRadius() * 2f);
            _lastHitTime = Time.time;
            return;
        }

        if (Time.time - _lastHitTime < HitCooldown)
        {
            return;
        }
        _lastHitTime = Time.time;

        // Falls, burning and poison have no hit point or direction, so those show on the player's centre.
        Vector3 point = hit.m_point != Vector3.zero ? hit.m_point : player.GetCenterPoint();
        Quaternion rotation = hit.m_dir.sqrMagnitude > 0.001f
            ? Quaternion.LookRotation(-hit.m_dir)
            : player.transform.rotation;
        Spawn(HitPrefab, point, rotation, 1f);
    }

    private static void Spawn(string prefabName, Vector3 position, Quaternion rotation, float scale)
    {
        GameObject prefab = ZNetScene.instance.GetPrefab(prefabName);
        if (prefab == null)
        {
            return;
        }

        GameObject effect = Object.Instantiate(prefab, position, rotation);
        effect.transform.localScale = new Vector3(scale, scale, scale);
    }
}
