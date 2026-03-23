# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

EctGodotUtils is a GDScript utility library for Godot 4.6, providing reusable components for game development (debug overlays, camera effects, audio management, scene transitions, procedural animation, etc.). It can be used as a project template or integrated by copying the `util/` folder into another project.

## Running the Project

Open in Godot 4.6+. The main scene is `res://test/test_main.tscn`, which auto-discovers test scenes in `test/` subdirectories. There is no external build system or test runner — testing is done by running scenes in the editor.

## Architecture

### Autoload Singletons

Most utilities are autoloads accessed globally. The required autoloads and their order are defined in `project.godot`:

- `DebugOverlayDisplay`, `InWorldDebugDisplay`, `DebugOverlay`, `AudioSettings`, `CameraEffects`, `PauseMenu`, `TransitionLayer`, `GlobalSounds`

### Code Organization

- `util/` — The library itself. Each feature has its own subdirectory (e.g., `util/camera/`, `util/audio/`). Scene files (`.tscn`) are colocated with their scripts.
- `test/` — Test/demo scenes. Each subdirectory contains a `*_test.tscn` that demonstrates a utility. New test scenes are auto-discovered by `generate_test_buttons.gd`.

## GDScript Conventions

This project follows the [official GDScript style guide](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html). Additionally:

- Type hints on all variables, parameters, and return types
- `class_name` for classes that need global registration
- `@export` for inspector properties, `@onready` for node references
- `##` doc comments for public API documentation
- `const` with `preload()` for scene/script references
- Snake_case for files/variables/methods, PascalCase for class names, UPPER_CASE for constants
- Signals for event communication between components

## Key Patterns

- **Autoload access**: utilities called as singletons (e.g., `CameraEffects.add_trauma()`, `DebugOverlay.add_message()`)
- **Coroutines**: `await` used for scene transitions and timed operations

## Branch History

The `3.x-mono` and `4.x-dotnet` branches contain older C# versions. The current `main` branch is pure GDScript targeting Godot 4.6.
