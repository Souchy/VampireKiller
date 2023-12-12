using Godot;

namespace VampireKiller;


public partial class Creature_res : Resource
{
    [Export]
    public int lifeMax { get; set; }

    [Export]
    public Texture2D icon { get; set; }

    [Export]
    public Mesh mesh { get; set; }

}
