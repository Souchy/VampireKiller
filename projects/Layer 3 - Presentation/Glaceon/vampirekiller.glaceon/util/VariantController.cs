using Godot;
using Godot.Sharp.Extras;
using System;

[Tool]
public partial class VariantController : Node
{
    [Export]
    public NodePath variantParentPath;
    [Export]
    public String variant;

    // public override void _Process(double delta)
    // {
    //     base._Process(delta);
    //     // updateVariant(variant);

    // }

    public override string[] _GetConfigurationWarnings()
    {
        if (Engine.IsEditorHint())
        {
            updateVariant(variant);
        }
        return base._GetConfigurationWarnings();
    }

    public void updateVariant()
    {
        updateVariant(variant);
    }

    public void updateVariant(String newVariant)
    {
        if (newVariant == null || variantParentPath == null || variantParentPath == "")
        {
            return;
        }

        Node variantParent = this.GetNodeOrNull(variantParentPath);
        if (variantParent == null)
        {
            return;
        }
        
        String? mostSimilarNode = null;
        foreach (MeshInstance3D node in variantParent.GetChildren<MeshInstance3D>())
        {
            if (node != null)
            {
                node.Visible = false;
                if (node.Name.ToString().ToLower().Contains(newVariant.ToLower()))
                {
                    mostSimilarNode = node.Name;
                }
            }
        }

        if (mostSimilarNode != null)
        {
            variantParent.GetNode<MeshInstance3D>(mostSimilarNode).Visible = true;
        }
    }
}
