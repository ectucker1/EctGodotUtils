﻿# ECT Godot Utils

This repository contains a lot of common utilities I tend to rewrite for Godot game projects.
They are all written in C# and thus exclusive to the Mono distribution of Godot.
They are currently in the process of being converted to Godot 4.0,
see the [3.x](https://github.com/ectucker1/EctGodotUtils/tree/3.x) branch for the 3.0 version.

Some examples of utilities included (not comprehensive):
- State machines
- LiveValues plugin for changing constants using reflection
- Debug overlays
- Verlet animations
- C# Optional Type
- Screenshake
- Coroutines using aync functions

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
