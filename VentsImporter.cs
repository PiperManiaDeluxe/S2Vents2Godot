using System;
using Godot;
using Godot.Collections;

public partial class VentsImporter : EditorImportPlugin
{
    public enum VentsImporterPreset
    {
        Source2Default
    }

    public override float _GetPriority()
    {
        return 1.0f;
    }

    public override string _GetImporterName()
    {
        return "vents.importer.sharp";
    }

    public override string _GetVisibleName()
    {
        return "Source 2 VENTS as scene import plugin";
    }

    public override string[] _GetRecognizedExtensions()
    {
        return new[] { "vents" };
    }

    public override string _GetResourceType()
    {
        return "PackedScene";
    }

    public override string _GetSaveExtension()
    {
        return ".tscn";
    }

    public override int _GetPresetCount()
    {
        return Enum.GetNames(typeof(VentsImporterPreset)).Length;
    }

    public override string _GetPresetName(int presetIndex)
    {
        switch (presetIndex)
        {
            case (int)VentsImporterPreset.Source2Default:
                return "Source 2 defualts";
            default:
                return "Undefined preset";
        }
    }

    public override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        switch (presetIndex)
        {
            case (int)VentsImporterPreset.Source2Default:
                Dictionary worldScaleOption = new()
                {
                    { "name", "World scale" },
                    { "default_value", 0.0254f }
                };
                Dictionary playerStartPathOption = new()
                {
                    { "name", "Player start resource path" },
                    { "default_value", "res://addons/S2Vents2Godot/Resources/info_player_start.tscn" }
                };
                Dictionary lightStrengthMultiplierOption = new()
                {
                    { "name", "Light strength multiplier" },
                    { "default_value", 1 }
                };

                return new Array<Dictionary> { worldScaleOption, playerStartPathOption, lightStrengthMultiplierOption };
            default:
                return new Array<Dictionary>();
        }
    }

    public override bool _GetOptionVisibility(string path, StringName optionName, Dictionary options)
    {
        return true;
    }

    public override int _GetImportOrder()
    {
        return 1;
    }

    public override Error _Import(string sourceFile, string savePath, Dictionary options,
        Array<string> platformVariants, Array<string> genFiles)
    {
        FileAccess file = FileAccess.Open(sourceFile, FileAccess.ModeFlags.Read);

        Node3D rootNode = new Node3D();
        string rootNodeName = "root";

        Dictionary currentNodeData = new Dictionary();
        int nodeIndex = -1;

        // Read all entity entries
        if (file.IsOpen())
        {
            while (!file.EofReached())
            {
                string line = file.GetLine().StripEdges();

                if (line.StartsWith("====") && line.EndsWith("===="))
                {
                    if (nodeIndex != -1)
                    {
                        if (rootNodeName == "root")
                            rootNodeName = "vents_" + (string)currentNodeData["worldname"];

                        CreateNodeFromData(rootNode, currentNodeData, options);
                    }

                    string prefix = "====";
                    string suffix = "====";
                    int indexStr = line.Substring(prefix.Length, line.Length - (prefix.Length + suffix.Length)).ToInt();
                    nodeIndex = indexStr;
                    currentNodeData.Clear();
                }

                var split = line.Split(" ", 3, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length >= 2)
                {
                    string key = split[0];
                    string value = "";
                    if (split.Length > 2)
                        value = split[2];
                    currentNodeData[key] = value;
                }
            }
        }
        else
            return Error.FileCantOpen;

        file.Close();

        if (nodeIndex != -1)
            CreateNodeFromData(rootNode, currentNodeData, options);

        rootNode.RotationDegrees = new Vector3(0, 180, 0);
        rootNode.Name = rootNodeName;

        PackedScene scene = new PackedScene();
        scene.Pack(rootNode);

        ResourceSaver.Save(scene, savePath + "." + _GetSaveExtension());

        return Error.Ok;
    }

    public void CreateNodeFromData(Node3D parentNode, Dictionary nodeData, Dictionary options)
    {
        Node3D newNode;

        switch ((string)nodeData["classname"])
        {
            case "info_player_start":
                newNode = InfoPlayerStart.CreateInfoPlayerStart(nodeData, options);
                break;
            case "light_omni":
                newNode = LightOmni.CreateLightOmni(nodeData, options);
                break;
            default:
                newNode = new Node3D();
                break;
        }

        newNode.Name = (string)nodeData["targetname"];

        if (newNode.Name == "")
        {
            newNode.Name = (string)nodeData["classname"];
        }

        newNode.Position = ParseVec3((string)nodeData["origin"]) * (float)options["World scale"];
        newNode.RotationDegrees = ParseVec3((string)nodeData["angles"]);
        newNode.RotationDegrees += new Vector3(0, -90, 0);
        newNode.Scale = ParseVec3((string)nodeData["scales"]);

        parentNode.AddChild(newNode);
        newNode.Owner = parentNode;
    }

    public static Vector3 ParseVec3(string vecString)
    {
        string[] split = vecString.Split(' ');
        if (split.Length < 3)
        {
            return Vector3.Zero;
        }

        return new Vector3(
            split[0].ToFloat(),
            split[2].ToFloat(),
            split[1].ToFloat()
        );
    }

    public static Vector4 ParseArray4(string vecString)
    {
        string[] split = vecString.Split('[')[1].Split(']')[0].Split(", ");
        if (split.Length < 4)
        {
            return Vector4.Zero;
        }

        return new Vector4(
            split[0].ToFloat(),
            split[1].ToFloat(),
            split[2].ToFloat(),
            split[3].ToFloat()
        );
    }
}