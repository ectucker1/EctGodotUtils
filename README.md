# ECT Godot Utils

This repository contains common utilities I tend to rewrite for Godot game projects,
particularly game jams.

They are provided under a MIT license for anyone else who may find them useful.

They are written in GDscript and currently target Godot 4.3.

Some examples of utilities included (not comprehensive):
- Debug overlay for printing text and displaying vectors
- LiveValues overlay for changing constants
- Screenshake, hitstop, and camera kickback
- Verlet physics for hair and rope
- Scene transitions
- Audio sequences
- Pause menus

Example usages of these can be found in the `test` folder.

## Installation in Existing Project

1. Copy the `util` folder into your project (you can rename util if you want).
2. Add the following as Godot Autoloads, in order:
    1. LiveValues (`res://util/live_values/live_values.gd`)
    2. DebugOverlayDisplay (`res://util/debug_overlay/debug_overlay_display.tscn`)
    3. InWorldDebugDisplay (`res://util/debug_overlay/in_world_overlay.tscn`)
    4. DebugOverlay (`res://util/debug_overlay/debug_overlay.tscn`)
    5. AudioSettings (`res://util/audio/audio_settings.gd`)
    6. CameraEffects (`res://util/camera/camera_effects.gd`)
    7. PauseMenu (`res://util/pause_menu/pause_menu.tscn`)
    8. TransitionLayer (`res://util/transition/transition_layer.tscn`)
    9. GlobalSounds (`res://util/audio/global_sounds.tscn`)

## Use as Template

This repo can also be used as a project template, which is useful for applying common engine settings.

1. Download the repo.
2. Change the project name in Project Settings.
3. (If using gitlab CI) Change the `EXPORT_NAME` variable in `.gitlab-ci.yml`.
