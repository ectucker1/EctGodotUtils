# ECT Godot Utils

This repository contains a lot of common utilities I tend to rewrite for Godot game projects.
They are all written in C# and thus exclusive to the Mono distribution of Godot.
They are currently tested on Godot 3.5.1, but should be compatible with newer Godot 3 versions.

Some examples of utilities included (not comprehensive):
- State machines
- LiveValues plugin for changing constants using reflection
- Debug overlays
- Verlet animations
- C# Optional Type
- Screenshake
- Coroutines using aync functions

The repository also contains copies of a couple other addons that pair well with it:
- [GodotExtraMath](https://github.com/aaronfranke/GodotExtraMath)
- [godot-plugin-refresher](https://github.com/godot-extended-libraries/godot-plugin-refresher)

## Installation in Existing Project

1. Copy the `addons` and `util` folders into your project (you can rename util if you want).
2. Build your Godot C# project.
3. Enable the LiveValues plugin.
4. Add the LiveValuesClient script as a autoload.

## Use as Template

This repo can also be used as a project template, which is useful for applying common engine settings.

1. Download the repo.
2. Change the project name in Project Settings.
3. (If using gitlab CI) Change the `EXPORT_NAME` variable in `.gitlab-ci.yml`.
