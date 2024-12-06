#if TOOLS
using Godot;

[Tool]
public partial class Vents : EditorPlugin
{
    private EditorImportPlugin _ventsImporterPlugin;

    public override void _EnterTree()
    {
        var script = GD.Load<Script>("res://addons/ventsimportersharp/VentsImporter.cs");
        _ventsImporterPlugin = new VentsImporter();
        _ventsImporterPlugin.SafelySetScript<Script>(script);
        
        AddImportPlugin(_ventsImporterPlugin);
    }

    public override void _ExitTree()
    {
        RemoveImportPlugin(_ventsImporterPlugin);
        _ventsImporterPlugin.Dispose();
    }
}
#endif