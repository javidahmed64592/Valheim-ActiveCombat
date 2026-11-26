# Changelog

## 0.2.1

- **Actions:** sheathing or drawing your weapons no longer keeps you walking and unable to attack for about a second: you can run and attack straight away while the animation plays. `SheatheTimeMultiplier` sets how much of the vanilla wait is kept (0 by default, 1 is vanilla).

## 0.2.0

- **Movement:** you slide along creatures and other players you run into instead of sticking to them. Vanilla's friction between characters could hold you in place against an enemy slightly in your way, especially on slopes.
- **Skills:** the combat skills (weapons, Blocking, magic, Run, Jump, Swim, Sneak and Dodge) need less experience for later levels: level 50 takes 78% of vanilla's total and level 100 72%, while early levels stay close to vanilla. Other skills are unchanged.
- **Skills:** weapon, magic and blood magic summon hits earn experience by the damage they really do (after resistances, capped at the target's remaining health), from about 0.7 times vanilla with early weapons to over twice late game. Blocks earn by the damage blocked. Starred creatures give 50% more per star, bosses double, and each extra creature hit by the same swing adds half its own experience.
- **Skills:** a dodge with an enemy that has spotted you within 15 metres gives 20 times vanilla's Dodge experience, and a perfect dodge 4 times. Experience now comes when the dodge starts, so pressing dodge with no stamina gives nothing.
- **Skills:** dying no longer costs combat skill levels. Instead you lose part of your progress towards the next level, never going below the start of your current level: 80% of a level on the Hard death penalty setting, 53% on Normal, 27% on Easy and 8% on Very Easy or Casual. Hardcore still resets every skill.

## 0.1.2

- Stamina regen during attacks and dodges is now earned with skill: it grows with your skill in the weapon in hand (Unarmed when punching) or Dodge, from none at skill 0 to half speed at skill 100. Early swings and punches no longer refund most of their own cost.

## 0.1.1

- The mod now shows as "Active Combat" in the BepInEx log and mod managers (it still said "Combat Flow" after the rename).

## 0.1.0

First release.

- **Actions and weapons:** movement no longer cancels equipping or reloading, gear swaps are instant, and crossbows, the grappling hook and Dundr load in half the time without using stamina.
- **Stamina and eitr:** stamina regenerates after half the vanilla delay and keeps coming back at half speed during attacks and dodges. Stamina and eitr regen grow with your skills, with diminishing returns alongside Rested.
- **Food:** eating fills your bars straight away, food restores stamina and eitr as well as health, and regen flows in smoothly instead of every 10 seconds.
- **Adrenaline:** bigger gains from hits, combos and hard hits, slower draining that improves with your weapon skill, and a reset to 30% instead of empty when a trinket fires.
- **Magic:** blood magic costs half as much health, and a mana shield pays a quarter of the damage you take from eitr, with effects when it takes a hit and when it breaks.
- **Falling:** a jump's height no longer adds to fall damage, feather fall only counts the fall after it ends, and short falls hurt less.
