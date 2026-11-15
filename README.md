[![CI](https://img.shields.io/github/actions/workflow/status/javidahmed64592/valheim-combatflow/ci.yml?branch=main&style=flat-square&label=CI&logo=github)](https://github.com/javidahmed64592/valheim-combatflow/actions/workflows/ci.yml)
[![Package](https://img.shields.io/github/actions/workflow/status/javidahmed64592/valheim-combatflow/package.yml?branch=main&style=flat-square&label=Package&logo=github)](https://github.com/javidahmed64592/valheim-combatflow/actions/workflows/package.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# CombatFlow

Smoother, more active combat: actions that are not cancelled by movement, instant gear swaps and faster stamina recovery.

## Features

- **Keep moving while you act.** Running, jumping and dodging no longer cancel equipping or reloading, so you can load a crossbow or grappling hook on the run.
- **Instant gear swaps.** Armour and weapons are put on and taken off straight away.
- **Faster reloads.** Crossbows, the grappling hook and Dundr load in half the time, on top of the vanilla skill bonus.
- **Stamina that comes back.** Loading a crossbow no longer stops stamina regenerating, and stamina starts regenerating after half the vanilla delay.

## Configuration

Settings are in `BepInEx/config/javidahmed64592.combatflow.cfg`. Every setting is marked "(Synced with server)": when the server has the mod, its values are used.

## Compatibility

- Other mods that change equip times, reload times or stamina regeneration may conflict with this mod.
- This mod has not been tested with a gamepad, only with keyboard and mouse.

## Uninstalling

Nothing is saved to your world or character, so the mod can be removed at any time.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/javidahmed64592/Valheim-CombatFlow/blob/main/LICENSE) file for details.
