[![CI](https://img.shields.io/github/actions/workflow/status/javidahmed64592/valheim-activecombat/ci.yml?branch=main&style=flat-square&label=CI&logo=github)](https://github.com/javidahmed64592/valheim-activecombat/actions/workflows/ci.yml)
[![Package](https://img.shields.io/github/actions/workflow/status/javidahmed64592/valheim-activecombat/package.yml?branch=main&style=flat-square&label=Package&logo=github)](https://github.com/javidahmed64592/valheim-activecombat/actions/workflows/package.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# ActiveCombat

Smoother, more active combat: act on the move, instant gear swaps, faster reloads, food that heals straight away, skill-based regen, combo-driven adrenaline, fairer falls, and cheaper blood magic with a mana shield.

Combat stays just as dangerous, but you spend less time waiting on your stamina bar and more time fighting.

## Actions and Weapons

- Running, jumping and dodging no longer cancel equipping or reloading, so you can load a crossbow on the run.
- Armour and weapons go on and come off instantly.
- Crossbows, the grappling hook and Dundr load in half the time, on top of the vanilla skill bonus.
- Loading a crossbow no longer uses stamina, so stamina keeps regenerating while you load.

## Movement

- You slide along creatures and other players you run into instead of sticking to them, so one slightly in your way can no longer hold you in place on a slope.

## Stamina and Eitr

- Stamina starts regenerating after half the vanilla delay.
- Stamina keeps coming back during attacks and dodges, instead of waiting for the whole animation to end. How much depends on your skill with the weapon in hand (Unarmed when punching) or Dodge: almost none for a beginner, up to half speed at skill 100.
- Stamina regenerates faster as your Run, Jump, Swim, Sneak and Dodge skills grow, and eitr as your best magic skill grows, up to +50% at skill 100.
- These skill bonuses have diminishing returns alongside Rested, so the two don't simply add up.
- Swimming and being over-encumbered still stop stamina regen.

## Food

- Eating heals you and fills your stamina and eitr straight away, by however much the food raised each bar.
- Food restores stamina and eitr over time as well as health, shown next to the health regen in its tooltip.
- Food regen flows in smoothly instead of arriving in one jump every 10 seconds.

## Adrenaline

- Hits give three times as much adrenaline.
- Quick combos build it faster, and harder hits such as backstabs, weak spots and heavy attacks give more.
- It waits twice as long before draining, and drains slower the better you are with the weapon in hand.
- When it triggers a trinket, it drops to 30% instead of empty.

## Magic

- Blood magic staffs cost half as much health to cast.
- **Mana shield:** while you have eitr, a quarter of the damage you take is paid from eitr instead of health, giving mages the defence their light armour lacks. A shimmer shows each hit it takes, and it shatters when a hit drains the last of your eitr.

## Falling

- Jumping off a ledge no longer adds your jump's height to the fall, and ending feather fall in mid-air only counts the fall after it ended.
- Short falls hurt less, while falls of 20 metres or more still do the full 100 damage.

## Skill Tracker

- Whenever you earn skill experience, the skill appears below the minimap with its icon, level and progress towards the next level, and the experience you earned rises beside it.
- Up to three skills show at once, each fading out a few seconds after its last gain. The level flashes gold when it goes up.
- Each part can be turned off in the config.

## Configuration

Settings are in `BepInEx/config/javidahmed64592.activecombat.cfg`.
Settings marked "(Synced with server)" use the server's values when the server has the mod installed.
Every change above can be tuned or turned off.

## Compatibility

- Other mods that change equip times, reload times, food, stamina and eitr regeneration, adrenaline, magic costs, fall damage or damage taken may conflict with this mod.
- This mod has not been tested with a gamepad, only with keyboard and mouse.

## Uninstalling

Nothing is saved to your world or character, so the mod can be removed at any time.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/javidahmed64592/Valheim-ActiveCombat/blob/main/LICENSE) file for details.
