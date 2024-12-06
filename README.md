# S2Vents2Godot importer

Enables importing Source 2 decompiled .vents files into Godot as a scene

## How to use

Firstly after [installing the plugin](https://docs.godotengine.org/en/stable/tutorials/plugins/editor/installing_plugins.html) open your Source 2 map's .vpk using [Source 2 Viewer](https://valveresourceformat.github.io/). Then locate `default_ents.vents_c` in your vpk, it will usally be in `[MAP NAME].vpk/maps/[MAP NAME]/entites`. \
Once you have found your vents_c file right click it in the viewer then choose `Decompile & Export`, export the file as a .vents file anywhere in your godot project. \
Finally you can import the .vents file as you would any other Godot PackedScene resource.

## Supported entity classes

- info_player_start
- light_omni
- more too come soon!
