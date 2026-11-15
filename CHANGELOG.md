# Changelog

## 0.1.0

- Running, jumping and dodging no longer cancel equipping or reloading. In vanilla, sprinting with stamina left wipes the action queue every frame, so a crossbow never loads while you run.
- Gear is put on and taken off instantly. Vanilla takes 1 to 2 seconds for armour and 0.2 seconds for weapons.
- Crossbows, the grappling hook and Dundr load in half the time. The vanilla skill bonus still applies on top, so a crossbow takes 1.75 seconds at skill 0 and under 1 second at skill 100. Dundr's eitr cost drops with its charge time, since it drains eitr per second while charging.
- Loading a crossbow no longer uses stamina. Vanilla only drains 1 stamina per second, but it resets the regen delay every frame, so stamina never regenerates while you load.
- Stamina starts regenerating 0.5 seconds after you stop using it, instead of 1 second.
