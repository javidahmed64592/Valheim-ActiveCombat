using ActiveCombat.Core;
using ActiveCombat.Movement;

using HarmonyLib;

namespace ActiveCombat.Patches;

/// <summary>Tracks every character's rigidbody so player contacts with them can be told apart from the ground.</summary>
[HarmonyPatch(typeof(Character), nameof(Character.Awake))]
[Feature(CharacterContacts.Feature)]
internal static class Character_Awake_ContactsPatch
{
    private static void Postfix(Character __instance)
    {
        CharacterContacts.Register(__instance);
    }
}

[HarmonyPatch(typeof(Character), nameof(Character.OnDestroy))]
[Feature(CharacterContacts.Feature)]
internal static class Character_OnDestroy_ContactsPatch
{
    private static void Postfix(Character __instance)
    {
        CharacterContacts.Unregister(__instance);
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.SetLocalPlayer))]
[Feature(CharacterContacts.Feature)]
internal static class Player_SetLocalPlayer_ContactsPatch
{
    private static void Postfix(Player __instance)
    {
        CharacterContacts.SetLocalPlayer(__instance);
    }
}
