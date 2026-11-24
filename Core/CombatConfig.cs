using BepInEx.Configuration;

namespace ActiveCombat.Core;

internal static class CombatConfig
{
    public static ConfigEntry<bool> MovementKeepsActions;
    public static ConfigEntry<float> EquipTimeMultiplier;
    public static ConfigEntry<float> ReloadTimeMultiplier;
    public static ConfigEntry<float> ReloadStaminaMultiplier;

    public static ConfigEntry<bool> SlidePastCharacters;

    public static ConfigEntry<float> StaminaRegenDelayMultiplier;
    public static ConfigEntry<float> AgilityRegenBonus;
    public static ConfigEntry<float> StaminaRegenWhileActing;

    public static ConfigEntry<float> MagicRegenBonus;
    public static ConfigEntry<float> SkillStackShare;

    public static ConfigEntry<bool> FoodFillsBars;
    public static ConfigEntry<bool> FoodContinuousRegen;
    public static ConfigEntry<float> FoodStaminaRegen;
    public static ConfigEntry<float> FoodEitrRegen;

    public static ConfigEntry<bool> FairFallDamage;
    public static ConfigEntry<float> FallDamageCurve;

    public static ConfigEntry<float> AdrenalineHitGain;
    public static ConfigEntry<float> AdrenalineComboWindow;
    public static ConfigEntry<float> AdrenalineComboBonus;
    public static ConfigEntry<float> AdrenalineComboMax;
    public static ConfigEntry<float> AdrenalineDamageScaling;
    public static ConfigEntry<float> AdrenalineDamageGainCap;
    public static ConfigEntry<float> AdrenalineDecayDelayMultiplier;
    public static ConfigEntry<float> AdrenalineSkillDecayReduction;
    public static ConfigEntry<float> AdrenalineProcReset;

    public static ConfigEntry<float> LevelCurveExponent;
    public static ConfigEntry<float> LevelCurveScale;
    public static ConfigEntry<float> DeathProgressLoss;
    public static ConfigEntry<float> XpDamageReference;
    public static ConfigEntry<float> XpDamageScaling;
    public static ConfigEntry<float> XpStarBonus;
    public static ConfigEntry<float> XpBossMultiplier;
    public static ConfigEntry<float> XpExtraTargetShare;

    public static ConfigEntry<float> BloodMagicHealthMultiplier;
    public static ConfigEntry<float> ManaShieldShare;
    public static ConfigEntry<float> ManaShieldEitrPerDamage;
    public static ConfigEntry<bool> ManaShieldEffects;

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

        SlidePastCharacters = config.Bind("Movement", "SlidePastCharacters", true,
            new ConfigDescription("You slide along creatures and other players you run into instead of sticking to them, so one slightly in your way can no longer hold you in place, even on a slope. They still block you head-on. (Synced with server)",
                null, Synced()));

        StaminaRegenDelayMultiplier = config.Bind("Stamina", "RegenDelayMultiplier", 0.5f,
            new ConfigDescription("Multiplies how long stamina waits after you use it before it starts regenerating. Vanilla waits 1 second; 0 starts straight away. (Synced with server)",
                new AcceptableValueRange<float>(0f, 2f), Synced()));
        StaminaRegenWhileActing = config.Bind("Stamina", "RegenWhileActing", 0.5f,
            new ConfigDescription("Share of your normal stamina regen you keep while attacking or dodging, earned with skill: it grows with your skill in the weapon in hand (Unarmed when punching) or Dodge, and reaches this value at 100, so a beginner gets almost none. Vanilla stops regen until the attack or dodge animation ends, even after the regen delay is over; 0 is vanilla, 1 regenerates at the full rate at skill 100. Swimming and being over-encumbered still stop regen. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        AgilityRegenBonus = config.Bind("Stamina", "AgilitySkillRegenBonus", 0.5f,
            new ConfigDescription("Extra stamina regen from the Run, Jump, Swim, Sneak and Dodge skills. It grows with their average level and reaches this value when all are 100, so 0.5 is +50%. Without Rested it is the least you get; with Rested see Regen.SkillStackShare. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        MagicRegenBonus = config.Bind("Eitr", "MagicSkillRegenBonus", 0.5f,
            new ConfigDescription("Extra eitr regen from your best magic skill (Elemental or Blood Magic). It grows with that skill and reaches this value at 100, so 0.5 is +50%. Without Rested it is the least you get; with Rested see Regen.SkillStackShare. Gear regen still adds in full. (Synced with server)",
                new AcceptableValueRange<float>(0f, 3f), Synced()));

        SkillStackShare = config.Bind("Regen", "SkillStackShare", 0.25f,
            new ConfigDescription("How skill regen bonuses combine with Rested and other regen effects. The bigger bonus counts in full and the smaller at this share, so Rested (+100%) and a +50% skill bonus give +112.5% at 0.25. 0 uses only the bigger one, 1 adds both in full. (Synced with server)",
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

        FairFallDamage = config.Bind("Falling", "FairFallDamage", true,
            new ConfigDescription("Fall damage counts how far below where you left the ground you land, and how fast you hit it, instead of the highest point you reached. Jumping off a ledge no longer adds your jump's height, and ending feather fall in mid-air only counts the fall after it ended. Falls under 4 metres still do no damage. (Synced with server)",
                null, Synced()));

        FallDamageCurve = config.Bind("Falling", "FallDamageCurve", 1.25f,
            new ConfigDescription("Shapes fall damage between 4 metres (none) and 20 metres (the full 100). 1 is vanilla's straight line. 1.25 makes short falls gentler while long falls stay deadly: 8 metres does 18 damage instead of 25, 12 metres 42 instead of 50, 16 metres 70 instead of 75. Up to 2 for gentler still. (Synced with server)",
                new AcceptableValueRange<float>(1f, 2f), Synced()));

        AdrenalineHitGain = config.Bind("Adrenaline", "HitGainMultiplier", 3f,
            new ConfigDescription("Multiplies the adrenaline each weapon or projectile hit gives (1 for most melee hits and 2 for arrows in vanilla), before the combo and damage bonuses. 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 10f), Synced()));
        AdrenalineComboWindow = config.Bind("Adrenaline", "ComboWindow", 3f,
            new ConfigDescription("Seconds you can wait between hits and keep your combo going. 0 turns the combo off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 10f), Synced()));
        AdrenalineComboBonus = config.Bind("Adrenaline", "ComboBonusPerHit", 0.25f,
            new ConfigDescription("Extra adrenaline from each hit in a combo after the first, so 0.25 makes the 2nd hit give 25% more, the 3rd 50% more and so on. Several targets hit by one swing count as one step. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        AdrenalineComboMax = config.Bind("Adrenaline", "ComboMaxMultiplier", 3f,
            new ConfigDescription("The most a combo can multiply a hit's adrenaline by. (Synced with server)",
                new AcceptableValueRange<float>(1f, 5f), Synced()));
        AdrenalineDamageScaling = config.Bind("Adrenaline", "DamageScaling", 0.5f,
            new ConfigDescription("How much harder hits give more adrenaline, measured against a normal hit from your weapon after the target's resistances, weak spots, backstabs and staggers. The gain is multiplied by (damage / normal) to this power, so at 0.5 a hit doing 4 times normal gives twice the adrenaline and a quarter of normal gives half. 0 turns it off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        AdrenalineDamageGainCap = config.Bind("Adrenaline", "DamageGainCap", 2f,
            new ConfigDescription("Limits the damage bonus: a hit gives at most this many times, and at least one over this many times, the adrenaline of a normal hit. (Synced with server)",
                new AcceptableValueRange<float>(1f, 4f), Synced()));
        AdrenalineDecayDelayMultiplier = config.Bind("Adrenaline", "DecayDelayMultiplier", 2f,
            new ConfigDescription("Multiplies how long adrenaline waits after you last gained some before it starts draining (6 to 10 seconds in vanilla, shorter when the bar is fuller). 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0.5f, 5f), Synced()));
        AdrenalineSkillDecayReduction = config.Bind("Adrenaline", "SkillDecayReduction", 0.75f,
            new ConfigDescription("How much your skill with the weapon in hand slows adrenaline draining. It grows with the skill and reaches this share at 100, so 0.75 drains at a quarter of the speed at skill 100. 0 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        AdrenalineProcReset = config.Bind("Adrenaline", "ProcResetShare", 0.3f,
            new ConfigDescription("Share of the bar adrenaline drops back to when it fills and triggers a trinket, so 0.3 leaves it 30% full. 0 empties it as in vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 0.9f), Synced()));

        LevelCurveExponent = config.Bind("Skills", "LevelCurveExponent", 1.4f,
            new ConfigDescription("How steeply the experience needed per level climbs for the combat skills (weapons, Blocking, magic, Run, Jump, Swim, Sneak and Dodge). Each level needs LevelCurveScale × (level + 1) to this power, plus 0.5. Vanilla is 1.5, which makes late levels very slow; 1.4 with a scale of 0.55 keeps early levels close to vanilla while level 50 needs 78% of vanilla's total and level 100 72%. Other skills stay vanilla. (Synced with server)",
                new AcceptableValueRange<float>(1f, 1.5f), Synced()));
        LevelCurveScale = config.Bind("Skills", "LevelCurveScale", 0.55f,
            new ConfigDescription("Multiplies the experience needed per level for the combat skills, see LevelCurveExponent. Vanilla is 0.5. (Synced with server)",
                new AcceptableValueRange<float>(0.1f, 2f), Synced()));
        DeathProgressLoss = config.Bind("Skills", "DeathProgressLoss", 0.8f,
            new ConfigDescription("Dying no longer costs combat skill levels. Instead each combat skill loses this share of a level's worth of progress towards its next level, never going below the start of its current level. This is the share on the Hard death penalty setting; the world setting scales it down: Normal two thirds of it (53% at 0.8), Easy a third (27%), Very Easy and Casual a tenth (8%). Hardcore still resets every skill. Other skills lose levels as in vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));

        XpDamageScaling = config.Bind("Skills", "XpDamageScaling", 0.5f,
            new ConfigDescription("How much harder hits give more experience, for weapon, magic and summon hits on creatures and for blocks. A hit's experience is multiplied by (damage / XpDamageReference) to this power, between 0.5 and 3 times vanilla's. Damage counts after the target's resistances, and only up to the health it had left, so overkill on weak creatures earns nothing extra. At 0.5 a flint weapon earns about 0.7 times vanilla and late game weapons over twice. 0 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        XpDamageReference = config.Bind("Skills", "XpDamageReference", 30f,
            new ConfigDescription("The damage of a hit that earns exactly vanilla's experience, see XpDamageScaling. (Synced with server)",
                new AcceptableValueRange<float>(5f, 200f), Synced()));
        XpStarBonus = config.Bind("Skills", "XpStarBonus", 0.5f,
            new ConfigDescription("Extra experience for hitting or blocking a starred creature, per star: 0.5 gives 1.5 times for one star and 2 times for two. (Synced with server)",
                new AcceptableValueRange<float>(0f, 2f), Synced()));
        XpBossMultiplier = config.Bind("Skills", "XpBossMultiplier", 2f,
            new ConfigDescription("Multiplies the experience for hitting or blocking a boss. (Synced with server)",
                new AcceptableValueRange<float>(1f, 5f), Synced()));
        XpExtraTargetShare = config.Bind("Skills", "XpExtraTargetShare", 0.5f,
            new ConfigDescription("Vanilla gives the same experience however many creatures one swing hits. Here the best hit counts in full and every other creature hit by the same swing adds this share of its own experience. 0 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));

        BloodMagicHealthMultiplier = config.Bind("Magic", "BloodMagicHealthMultiplier", 0.5f,
            new ConfigDescription("Multiplies the health blood magic staffs cost per cast (40 to 60% of your health in vanilla). 0.5 halves it, 0 removes the cost, 1 is vanilla. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        ManaShieldShare = config.Bind("Magic", "ManaShieldShare", 0.25f,
            new ConfigDescription("Share of the damage you take that is paid from eitr instead of health, while you have eitr. 0.25 is a quarter; 0 turns the shield off. (Synced with server)",
                new AcceptableValueRange<float>(0f, 1f), Synced()));
        ManaShieldEitrPerDamage = config.Bind("Magic", "ManaShieldEitrPerDamage", 1f,
            new ConfigDescription("Eitr used for each point of damage the shield takes. (Synced with server)",
                new AcceptableValueRange<float>(0.1f, 5f), Synced()));
        ManaShieldEffects = config.Bind("Magic", "ManaShieldEffects", true,
            new ConfigDescription("Shows a shimmer where the mana shield takes a hit, and a burst when a hit drains your eitr to nothing. Only affects your own character's effects."));
    }

    /// <summary>Settings that use the server's value when the server has the mod.</summary>
    private static ConfigurationManagerAttributes Synced()
    {
        return new ConfigurationManagerAttributes { IsAdminOnly = true };
    }
}
