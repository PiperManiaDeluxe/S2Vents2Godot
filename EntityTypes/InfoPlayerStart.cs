using Godot;
using Godot.Collections;

public static class InfoPlayerStart
{
    public static Node3D CreateInfoPlayerStart(Dictionary nodeData, Dictionary options)
    {
        string playerStartResourcePath = (string)options["Player start resource path"];
        PackedScene scene = (PackedScene)GD.Load(playerStartResourcePath);
        Node3D node = (Node3D)scene.Instantiate(PackedScene.GenEditState.Disabled);

        return node;
    }
}