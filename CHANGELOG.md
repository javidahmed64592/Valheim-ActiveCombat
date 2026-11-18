# Changelog

## 0.1.0

- Running, jumping and dodging no longer cancel equipping or reloading. In vanilla, sprinting with stamina left wipes the action queue every frame, so a crossbow never loads while you run.
- Gear is put on and taken off instantly. Vanilla takes 1 to 2 seconds for armour and 0.2 seconds for weapons.
- Crossbows, the grappling hook and Dundr load in half the time. The vanilla skill bonus still applies on top, so a crossbow takes 1.75 seconds at skill 0 and under 1 second at skill 100. Dundr's eitr cost drops with its charge time, since it drains eitr per second while charging.
- Loading a crossbow no longer uses stamina. Vanilla only drains 1 stamina per second, but it resets the regen delay every frame, so stamina never regenerates while you load.
- Stamina starts regenerating 0.5 seconds after you stop using it, instead of 1 second.
- Eating fills your bars by however much the food raised them. Health, stamina and eitr each go up by the amount their maximum rose, so a fresh meal heals you straight away instead of over the next minute or two. Topping up a half-eaten food fills by the difference.
- Stamina regenerates faster as your Run, Jump, Swim, Sneak and Dodge skills grow, up to +50% when all five are at 100. The bonus follows their average level.
- Eitr regenerates faster as your best magic skill grows, up to +50% at Elemental or Blood Magic 100. Only your best school counts, so you don't need to train both.
- Skill regen bonuses are the least you get without Rested, and have diminishing returns with it: the bigger of the two bonuses counts in full and the smaller at a quarter, so Rested (+100%) with a +50% skill bonus gives +112.5% rather than +150%. Set it with the synced `Regen.SkillStackShare` setting (0.25; 0 uses only the bigger bonus, 1 adds both).
- Food restores stamina and eitr over time as well as health. Each food's stamina and eitr regen is in the same proportion to its stamina and eitr as its health regen is to its health, so bread restores about 6 stamina every 10 seconds and smoked fish about 16 eitr. Foods that raise more than one bar restore each of them. The tooltip shows the amounts next to the health regen. Set them with the synced `Food.StaminaRegen` and `Food.EitrRegen` settings (1; 0 turns them off).
- Food regen flows in smoothly instead of arriving in one jump every 10 seconds, at the same amount per 10 seconds. Turn it off with the synced `Food.ContinuousRegen` setting.
- Blood magic staffs cost half as much health. In vanilla each cast takes 40 to 60% of your current health, so it now takes 20 to 30%. The item tooltip shows the lower cost. Set it with the new synced `Magic.BloodMagicHealthMultiplier` setting (0.5 by default, 0 removes the cost, 1 is vanilla).
- New mana shield: while you have eitr, a quarter of the damage you take comes out of your eitr instead of your health, at 1 eitr per point of damage. It covers every kind of damage, including falls, fire and poison, and pauses eitr regen like casting does. Players without eitr are unaffected. Set it with the new synced `Magic.ManaShieldShare` (0.25) and `Magic.ManaShieldEitrPerDamage` (1) settings.
