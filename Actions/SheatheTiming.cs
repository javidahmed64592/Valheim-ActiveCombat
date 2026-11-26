using ActiveCombat.Core;

using UnityEngine;

namespace ActiveCombat.Actions;

/// <summary>
/// <para>
/// Sheathing or drawing your weapons swaps the items at once, then plays an animation on the upper-body layer. That
/// animation is a minor action, so vanilla keeps you walking and unable to attack until it ends (about a second).
/// </para>
/// <para>
/// This follows the animation started by the local player's last sheathe or draw and reports it as over once it has
/// played <see cref="CombatConfig.SheatheTimeMultiplier"/> of its length, so at 0 you can run and attack straight
/// away while the animation still finishes on your upper body.
/// </para>
/// </summary>
internal static class SheatheTiming
{
    private const int UpperBodyLayer = 1;

    /// <summary>
    /// How long after the sheathe to wait for its animation to start before giving up on it.
    /// </summary>
    private const float StartWindow = 0.5f;

    private static readonly int MinorAction = Animator.StringToHash("minoraction");
    private static readonly int MinorActionFast = Animator.StringToHash("minoraction_fast");

    private static bool _following;
    private static float _startTime;
    private static int _state;

    /// <summary>
    /// Called when a character sheathes or draws its weapons with the animation.
    /// </summary>
    public static void Started(Humanoid humanoid)
    {
        if (humanoid != null && humanoid == Player.m_localPlayer)
        {
            _following = true;
            _startTime = Time.time;
            _state = 0;
        }
    }

    /// <summary>
    /// Whether a minor action vanilla reports for the player is the sheathe animation, played far enough to end.
    /// </summary>
    public static bool EndedEarly(Player player, Animator animator)
    {
        if (!_following || player != Player.m_localPlayer || animator == null)
        {
            return false;
        }

        float multiplier = CombatConfig.SheatheTimeMultiplier.Value;
        if (multiplier >= 1f)
        {
            return false;
        }

        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(UpperBodyLayer);
        bool transition = animator.IsInTransition(UpperBodyLayer);
        AnimatorStateInfo next = transition ? animator.GetNextAnimatorStateInfo(UpperBodyLayer) : default;

        // The first minor action state after the sheathe is its animation (while blending in, it's the next state).
        if (_state == 0)
        {
            if (transition && IsMinorAction(next))
            {
                _state = next.fullPathHash;
            }
            else if (IsMinorAction(current))
            {
                _state = current.fullPathHash;
            }
            else
            {
                _following = Time.time - _startTime < StartWindow;
                return false;
            }
        }

        AnimatorStateInfo state;
        if (transition && next.fullPathHash == _state)
        {
            state = next;
        }
        else if (current.fullPathHash == _state)
        {
            state = current;
        }
        else
        {
            // The animation is over (or something else replaced it).
            _following = false;
            return false;
        }

        return state.normalizedTime >= multiplier;
    }

    private static bool IsMinorAction(AnimatorStateInfo state)
    {
        return state.tagHash == MinorAction || state.tagHash == MinorActionFast;
    }
}
