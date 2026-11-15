using BepInEx.Configuration;

namespace CombatFlow.Core;

internal static class CombatConfig
{
    public static ConfigEntry<bool> MovementKeepsActions;
    public static ConfigEntry<float> EquipTimeMultiplier;
    public static ConfigEntry<float> ReloadTimeMultiplier;
    public static ConfigEntry<float> ReloadStaminaMultiplier;

    public static ConfigEntry<float> StaminaRegenDelayMultiplier;
    public static ConfigEntry<float> AgilityRegenBonus;

    public static ConfigEntry<float> MagicRegenBonus;

    public static ConfigEntry<bool> FoodFillsBars;

    public static void Bind(ConfigFile config)
    {
        MovementKeepsActions = config.Bind("Actions", "MovementKeepsActions", true,
            new ConfigDescription("Running, jumping and dodging no longer cancel equipping or reloading. (Synced with server)",
                null, Synced()));
        EquipTimeMultiplier = config.Bind("Actions", "EquipTimeMultiplier", 0f,
            new ConfigDescription("Multiplies how long gear takes to put on or take off. 0 makes it instant, 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 2f), Synced()));
        ReloadTimeMultiplier = config.Bind("Actions", "ReloadTimeMultiplier", 0.5f,
            new ConfigDescription("Multiplies how long crossbows, the grappling hook and Dundr take to load, after the vanilla skill bonus. 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0.1f, 2f), Synced()));
        ReloadStaminaMultiplier = config.Bind("Actions", "ReloadStaminaMultiplier", 0f,
            new ConfigDescription("Multiplies the stamina used while loading a crossbow. At 0 loading uses none, so stamina keeps regenerating while you load. 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 2f), Synced()));

        StaminaRegenDelayMultiplier = config.Bind("Stamina", "RegenDelayMultiplier", 0.5f,
            new ConfigDescription("Multiplies how long stamina waits after you use it before it starts regenerating. Vanilla waits 1 second; 0 starts straight away. (Synced with server)",
                new AcceptableValueRange<float>(0f, 2f), Synced()));
        AgilityRegenBonus = config.Bind("Stamina", "AgilitySkillRegenBonus", 0.5f,
            new ConfigDescription("Extra stamina regen from the Run, Jump, Swim, Sneak and Dodge skills. It grows with their average level and reaches this value when all are 100, so 0.5 is +50%. It stacks with Rested the same way. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        MagicRegenBonus = config.Bind("Eitr", "MagicSkillRegenBonus", 0.5f,
            new ConfigDescription("Extra eitr regen from your best magic skill (Elemental or Blood Magic). It grows with that skill and reaches this value at 100, so 0.5 is +50%. It stacks with gear and Rested the same way. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        FoodFillsBars = config.Bind("Food", "FoodFillsBars", true,
            new ConfigDescription("Eating straight away heals you, and fills your stamina and eitr, by however much the food raised each maximum. In vanilla only the maximum goes up and you have to wait for regen to catch up. (Synced with server)",
                null, Synced()));
    }

    /// <summary>Settings that use the server's value when the server has the mod.</summary>
    private static ConfigurationManagerAttributes Synced()
    {
        return new ConfigurationManagerAttributes { IsAdminOnly = true };
    }
}
