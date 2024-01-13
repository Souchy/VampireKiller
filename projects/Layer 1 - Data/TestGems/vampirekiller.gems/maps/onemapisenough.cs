using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using Util.communication.events;
using vampirekiller.logia.stub;
using VampireKiller.eevee.vampirekiller.eevee;

// Eevee
//public class Map
//{
//    public void spawnmachin()
//    {
//        //asdassdsd
//    }
//}

//// gems
//public class MapBossMachin : Map
//{

//}

//// Glaceon
//// .tscn


/// <summary>
/// TODO: inherit a MapNode with a NavigationRegion3D and the spawn() methods
/// </summary>
public partial class onemapisenough : Node3D
{
    // private System.Threading.Timer timer { get; set; }
    [NodePath]
    private Godot.Timer timer { get; set; }

    public onemapisenough()
    {
        EventBus.centralBus.subscribe(this);
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        //GD.Print("Map Enter tree base");
        if (Universe.fight == null)
            return;
        //GD.Print("Map Enter tree with fight");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        //this.timer.Timeout += spawn;
        //GD.Print("Map isReady");
    }

    [Subscribe(Fight.EventSet)]
    public void onFightChanged(Fight fight)
    {
        //GD.Print("Map: onFight");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        bool clicked = Input.IsActionJustPressed("spawn_enemy");
        if (clicked)
        {
            spawn();
        }
    }

    //[SignalHandler(nameof(Timer.Timeout), nameof(timer))]
    //public void _on_timer_timeout() => spawn();
    public void spawn()
    {
        if (Universe.fight != null)
        {
            var crea = StubFight.spawnStubCreature(new Godot.Vector3(7, 0, 7));
            Universe.fight.creatures.add(crea);
        }
    }

}
