# ECT Godot Utils

This repository contains a lot of common utilities I tend to rewrite for Godot game projects.
They are all written in C# and thus exclusive to the Mono distribution of Godot.
They are currently tested on Godot 3.4.2, but should be compatible with newer Godot 3 versions.

Utilities here include state machines, a plugin that changes constants as the game runs,
procedural verlet animations, a C# Optional type, and more.

## Installation

1. Copy the `addons` and `util` folders into your project (you can rename util if you want).
2. Build your Godot C# project
3. Enable the LiveValues plugin.
4. Add the LiveValuesClient script as a autoload.

The [godot-plugin-refresher](https://github.com/godot-extended-libraries/godot-plugin-refresher) plugin is also recommended,
to prevent needing to restart the editor for new live values.
