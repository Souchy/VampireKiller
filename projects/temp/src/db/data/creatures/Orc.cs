using Godot;
using VampireKiller.scenes;

namespace VampireKiller.db;

public partial class Orc : Creature
{
    public OrcModel model { get; } = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

public class OrcModel : CreatureModel
{
    public OrcModel()
    {
        this.stats.set(new CreatureLifeMax(1000));
        this.stats.set(new CreatureManaMax(10));
        this.stats.set(new CreaturelAttack(10));
        this.stats.set(new CreatureMovementSpeed(10));
        // this.stats.flat.get
    }
}
