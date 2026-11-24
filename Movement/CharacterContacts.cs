using System.Collections.Generic;

using ActiveCombat.Core;

using Unity.Collections;

using UnityEngine;

namespace ActiveCombat.Movement;

/// <summary>
/// Removes friction between the local player and other characters.
/// Vanilla gives every character's collider a friction of 0.4 while moving and 0.8 (combined as the maximum)
/// while standing still, so pushing into a creature grips it and stops you sliding along it, up and down a
/// slope as well as sideways. Unity's contact modification lets us zero the friction of just those contacts,
/// leaving the player's grip on the ground as it is.
/// </summary>
internal static class CharacterContacts
{
    /// <summary>
    /// Instance IDs of every character's rigidbody. Read on physics threads during the simulation step,
    /// and only written on the main thread between steps.
    /// </summary>
    private static readonly HashSet<int> CharacterBodies = [];

    private static int _playerCollider;

    public static void Subscribe()
    {
        Physics.ContactModifyEvent += OnContactModify;
    }

    public static void Unsubscribe()
    {
        Physics.ContactModifyEvent -= OnContactModify;
    }

    public static void Register(Character character)
    {
        if (character.m_body != null)
        {
            _ = CharacterBodies.Add(character.m_body.GetInstanceID());
        }
    }

    public static void Unregister(Character character)
    {
        if (character.m_body != null)
        {
            _ = CharacterBodies.Remove(character.m_body.GetInstanceID());
        }
    }

    /// <summary>Called when a player becomes the local player: only its collider reports contacts to modify.</summary>
    public static void SetLocalPlayer(Player player)
    {
        if (player.m_collider == null)
        {
            return;
        }
        player.m_collider.hasModifiableContacts = true;
        _playerCollider = player.m_collider.GetInstanceID();
    }

    private static void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
    {
        if (!CombatConfig.SlidePastCharacters.Value)
        {
            return;
        }
        for (int i = 0; i < pairs.Length; i++)
        {
            ModifiableContactPair pair = pairs[i];
            int other;
            if (pair.colliderInstanceID == _playerCollider)
            {
                other = pair.otherBodyInstanceID;
            }
            else if (pair.otherColliderInstanceID == _playerCollider)
            {
                other = pair.bodyInstanceID;
            }
            else
            {
                continue;
            }
            if (!CharacterBodies.Contains(other))
            {
                continue;
            }
            for (int c = 0; c < pair.contactCount; c++)
            {
                pair.SetStaticFriction(c, 0f);
                pair.SetDynamicFriction(c, 0f);
            }
        }
    }
}
