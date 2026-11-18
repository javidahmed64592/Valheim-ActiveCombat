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
    public static ConfigEntry<float> SkillStackShare;

    public static ConfigEntry<bool> FoodFillsBars;
    public static ConfigEntry<bool> FoodContinuousRegen;
    public static ConfigEntry<float> FoodStaminaRegen;
    public static ConfigEntry<float> FoodEitrRegen;

    public static ConfigEntry<float> BloodMagicHealthMultiplier;
    public static ConfigEntry<float> ManaShieldShare;
    public static ConfigEntry<float> ManaShieldEitrPerDamage;

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
            new ConfigDescription("Extra stamina regen from the Run, Jump, Swim, Sneak and Dodge skills. It grows with their average level and reaches this value when all are 100, so 0.5 is +50%. Without Rested it is the least you get; with Rested see Regen.SkillStackShare. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        MagicRegenBonus = config.Bind("Eitr", "MagicSkillRegenBonus", 0.5f,
            new ConfigDescription("Extra eitr regen from your best magic skill (Elemental or Blood Magic). It grows with that skill and reaches this value at 100, so 0.5 is +50%. Without Rested it is the least you get; with Rested see Regen.SkillStackShare. Gear regen still adds in full. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        SkillStackShare = config.Bind("Regen", "SkillStackShare", 0.5f,
            new ConfigDescription("How skill regen bonuses combine with Rested and other regen effects. The bigger bonus counts in full and the smaller at this share, so Rested (+100%) and a +50% skill bonus give +125% at 0.5. 0 uses only the bigger one, 1 adds both in full. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));

        FoodFillsBars = config.Bind("Food", "FoodFillsBars", true,
            new ConfigDescription("Eating straight away heals you, and fills your stamina and eitr, by however much the food raised each maximum. In vanilla only the maximum goes up and you have to wait for regen to catch up. (Synced with server)",
                null, Synced()));

        FoodContinuousRegen = config.Bind("Food", "ContinuousRegen", true,
            new ConfigDescription("Food restores health, stamina and eitr smoothly over time, instead of in one jump every 10 seconds. The amount per 10 seconds is the same. (Synced with server)",
                null, Synced()));
        FoodStaminaRegen = config.Bind("Food", "StaminaRegen", 1f,
            new ConfigDescription("Food also restores stamina and eitr every 10 seconds, alongside the health it restores in vanilla. A food restores stamina in the same proportion to its stamina as its health regen is to its health, times this value, so bread (23 health, 70 stamina, 2 hp per tick) restores about 6 stamina per tick at 1. 0 turns it off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 5f), Synced()));
        FoodEitrRegen = config.Bind("Food", "EitrRegen", 1f,
            new ConfigDescription("The same for eitr, for foods that raise eitr. 0 turns it off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 5f), Synced()));

        BloodMagicHealthMultiplier = config.Bind("Magic", "BloodMagicHealthMultiplier", 0f,
            new ConfigDescription("Multiplies the health blood magic staffs cost per cast (40 to 60% of your health in vanilla). 0 removes the cost, 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        ManaShieldShare = config.Bind("Magic", "ManaShieldShare", 0.25f,
            new ConfigDescription("Share of the damage you take that is paid from eitr instead of health, while you have eitr. 0.25 is a quarter; 0 turns the shield off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        ManaShieldEitrPerDamage = config.Bind("Magic", "ManaShieldEitrPerDamage", 1f,
            new ConfigDescription("Eitr used for each point of damage the shield takes. (Synced with server)",
                new AcceptableValueRange<float>(0.1f, 5f), Synced()));
    }

    /// <summary>Settings that use the server's value when the server has the mod.</summary>
    private static ConfigurationManagerAttributes Synced()
    {
        return new ConfigurationManagerAttributes { IsAdminOnly = true };
    }
}
