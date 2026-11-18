using CombatFlow.Core;

using UnityEngine;

namespace CombatFlow.Falling;

/// <summary>
/// Vanilla hurts you by how far you fell from the highest point you reached in the air, so a jump's rise counts
/// as falling, and so does the whole drop through feather fall if the effect ends before you land. This measures
/// the fall two ways and uses the smaller: how far below where you left the ground you landed (so jumping adds
/// nothing), and how high a free fall it would take to hit the ground as fast as you did (so slowed falling
/// counts only for the part after the slowing ended). It never makes a fall count for more than vanilla does.
/// Only the local player is tracked; their physics run on their own client.
/// </summary>
internal static class FallHeight
{
    private static float _takeOffHeight = float.NegativeInfinity;
    private static float _fastestFall;

    /// <summary>Called every physics step before vanilla checks for landing.</summary>
    public static void Update(Character character)
    {
        if (!CombatConfig.FairFallDamage.Value || character != Player.m_localPlayer)
        {
            return;
        }

        float height = character.transform.position.y;
        if (!character.m_groundContact)
        {
            _fastestFall = Mathf.Max(_fastestFall, -character.m_body.linearVelocity.y);
            return;
        }

        float vanilla = Mathf.Max(0f, character.m_maxAirAltitude - height);
        float fair = Mathf.Min(Mathf.Max(0f, _takeOffHeight - height), FreeFallHeight(_fastestFall));
        if (fair < vanilla)
        {
            character.m_maxAirAltitude = height + fair;
        }

        _takeOffHeight = height;
        _fastestFall = 0f;
    }

    /// <summary>The height a free fall from rest takes to reach this speed.</summary>
    private static float FreeFallHeight(float speed)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        return gravity > 0f ? speed * speed / (2f * gravity) : float.PositiveInfinity;
    }
}
