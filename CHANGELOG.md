# Changelog

## 2.4.0
- Updated to R.E.P.O v0.4.0 Cosmetic Update
- Colour opacity is now limited between 0.1 - 0.7 (was 0 - 0.5 before)
- The 'Match Skin Colour' setting has been renamed to 'Match Grabber Cosmetic' as it now uses the colour of your grabber cosmetic
- Fixed an issue that caused players without the mod to have invisible grab beams for players with the mod

## 2.3.0
- Updated to R.E.P.O v0.3.0 Monster Update (late i know, but the mod v2.2.0 worked fine mostly)
- Added the ability to customise the climbing beam colour when using the tumble climb upgrade

## 2.2.0
v2.2.0 includes a few bug fixes, small changes, and networking improvements
- Updated to R.E.P.O v0.2.0 Museum Update
- Changing the colour sliders with the match skin setting enabled no longer causes an incorrect preview colour to show
- The mod will no longer unnecessarily send grab beam colour updates every single time you use the grab beam. Should result in slightly better network latency compared to v2.1.0
- Added the grab beam colour settings button to the vanilla colour menu. This means you can now change the grab beam colour while waiting in the lobby
- Added a debug option to add the grab beam colour settings button to the main menu

## 2.1.0
v2.1.0 comes with a few small bug fixes and changes including:
- The default opacity for the neutral grab beam is now 0.35 to make it more visible with some skin colours
- Further improved the accuracy of the colour previews
- Fixed the grids when rotating an item being way too bright or becoming pure white with certain colours
- Fixed the internal plugin version incorrectly being `1.1.0`
- Fixed some spelling in the changelog and readme

## 2.0.1
Forgot to update the changelog, this version is identical to 2.0.0

## 2.0.0
v2 comes with a bunch of fixes and two much requested new features!

### New features / changes
- You can now customise the colour of your grab beam when rotating objects or healing other players!
   - Each beam type can be customised independently
- Added a new option that lets you automatically sync the beam colour to your skin colour
   - When this option is enabled, all settings for that beam type will be ignored apart from the opacity
   - Even works with the CustomColors mod!
- By default, the neutral grab beam will now match your skin colour
   - To disable, simply set "Match Skin Colour" to "off" in the config menu

### Fixes
- The colour preview in the config menu is now much more accurate
   - Previously a lot of brighter colours would just appear white in-game, this has been fixed
- The tip of the grab beam will now properly update
   - Previously it would stay as orange until you rotated something or healed a player
- Other players grab beams will no longer reset back to orange until they rotated something or healed a player when entering a new level
- Removed some error spam that could happen when other players changed their beam colour
  
## 1.1.0
- Updated the mod icon
- Made it so grabbing the cart will no longer force your grab beam to have 0.1 opacity until you rotated an item or healed another player

## 1.0.0
Initial release
