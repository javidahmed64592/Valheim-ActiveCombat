# Changelog

## Unreleased

- **Skills:** the combat skills (weapons, Blocking, magic, Run, Jump, Swim, Sneak and Dodge) need less experience for later levels: level 50 takes 60% of vanilla's total and level 100 about half. Other skills are unchanged.
- **Skills:** dying takes 2% of each combat skill instead of 5% (still scaled by the world's death penalty setting) and keeps your progress towards the next level.

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
