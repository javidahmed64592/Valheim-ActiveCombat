namespace ActiveCombat.Progression;

/// <summary>The skills this mod's progression changes apply to: weapons, blocking, magic and movement.
/// Gathering, crafting and other skills stay vanilla.</summary>
internal static class CombatSkills
{
    public static bool Contains(Skills.SkillType skill)
    {
        return skill switch
        {
            Skills.SkillType.Swords or Skills.SkillType.Knives or Skills.SkillType.Clubs or Skills.SkillType.Polearms
                or Skills.SkillType.Spears or Skills.SkillType.Blocking or Skills.SkillType.Axes or Skills.SkillType.Bows
                or Skills.SkillType.ElementalMagic or Skills.SkillType.BloodMagic or Skills.SkillType.Unarmed
                or Skills.SkillType.Crossbows or Skills.SkillType.Jump or Skills.SkillType.Sneak or Skills.SkillType.Run
                or Skills.SkillType.Swim or Skills.SkillType.Dodge => true,
            _ => false,
        };
    }
}
