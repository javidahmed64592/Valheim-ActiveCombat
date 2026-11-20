using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Falling;

/// <summary>
/// Vanilla hurts you by how far you fell from the highest point you reached in the air, so a jump's rise counts
/// as falling, and so does the whole drop through feather fall if the effect ends before you land. This measures
/// the fall two ways and uses the smaller: how far below where you left the ground you landed (so jumping adds
/// nothing), and how high a free fall it would take to hit the ground as fast as you did (so slowed falling
/// counts only for the part after the slowing ended). It never makes a fall count for more than vanilla does.
/// Falls over the safe height can also be scaled along a curve, so short falls hurt less while the height that
/// does full damage stays the same. Only the local player is tracked; their physics run on their own client.
/// </summary>
internal static class FallHeight
{
    /// <summary>Vanilla's fall damage starts above this height and reaches its full 100 at SafeHeight + DamageRange.</summary>
    private const float SafeHeight = 4f;
    private const float DamageRange = 16f;

    private static float _takeOffHeight = float.NegativeInfinity;
    private static float _fastestFall;

    /// <summary>Called every physics step before vanilla checks for landing.</summary>
    public static void Update(Character character)
    {
        if (character != Player.m_localPlayer)
        {
            return;
        }

        float height = character.transform.position.y;
        if (!character.m_groundContact)
        {
            _fastestFall = Mathf.Max(_fastestFall, -character.m_body.linearVelocity.y);
            return;
        }

        float fall = Mathf.Max(0f, character.m_maxAirAltitude - height);
        if (CombatConfig.FairFallDamage.Value)
        {
            fall = Mathf.Min(fall, Mathf.Max(0f, _takeOffHeight - height), FreeFallHeight(_fastestFall));
        }

        character.m_maxAirAltitude = height + Curve(fall);

        _takeOffHeight = height;
        _fastestFall = 0f;
    }

    /// <summary>
    /// Vanilla's damage is linear in the height fallen past the safe height. Raising that fraction to the curve's
    /// power before vanilla sees it gives (fraction ^ power) of the full damage instead.
    /// </summary>
    private static float Curve(float fall)
    {
        float power = CombatConfig.FallDamageCurve.Value;
        return fall <= SafeHeight || fall >= SafeHeight + DamageRange || Mathf.Approximately(power, 1f)
            ? fall
            : SafeHeight + (DamageRange * Mathf.Pow((fall - SafeHeight) / DamageRange, power));
    }

    /// <summary>The height a free fall from rest takes to reach this speed.</summary>
    private static float FreeFallHeight(float speed)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        return gravity > 0f ? speed * speed / (2f * gravity) : float.PositiveInfinity;
    }
}
