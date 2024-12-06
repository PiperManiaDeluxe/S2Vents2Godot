using Godot;
using Godot.Collections;

public static class LightOmni
{
    public static Node3D CreateLightOmni(Dictionary nodeData, Dictionary options)
    {
        bool bakeLightIndexing = (bool)nodeData["baked_light_indexing"];
        float range = (float)nodeData["range"] * (float)options["World scale"]; 
        float brightness = (float)nodeData["brightness"] * (float)options["Light strength multiplier"];
        Vector4 color = VentsImporter.ParseArray4((string)nodeData["color"]);
        bool enabled = (bool)nodeData["enabled"];
        
        OmniLight3D light = new OmniLight3D();
        if (bakeLightIndexing)
            light.LightBakeMode = Light3D.BakeMode.Static;
        light.OmniRange = range;
        light.LightEnergy = brightness;
        light.LightColor = Color.Color8((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
        light.Visible = enabled;

        return light;
    }
}