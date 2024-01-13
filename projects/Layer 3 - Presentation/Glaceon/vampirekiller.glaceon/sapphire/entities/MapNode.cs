using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.logia.stub;

namespace vampirekiller.glaceon.sapphire.entities;

public partial class MapNode : Node3D
{
    public const string EnemySpawnPointGroup = "EnemySpawnPoint";
    public const string PlayerSpawnPointGroup = "PlayerSpawnPoint";


    [NodePath]
    public Marker3D PlayerSpawnPoint { get; set; }
    private Marker3D[] enemySpawnPoints;
    /// <summary>
    /// In seconds
    /// </summary>
    private const double spawnTimer = 1;
    private double spawnTimerDelta = spawnTimer;
    private Random random = new Random();

    public override void _Ready()
    {
        this.OnReady();
        enemySpawnPoints = this.GetNodesOfType<Marker3D>().Where(c => c.IsInGroup(EnemySpawnPointGroup)).ToArray();
    }

    public override void _Process(double delta)
    {
        spawnTimerDelta -= delta;
        if(spawnTimerDelta <= 0)
        {
            spawnTimerDelta = spawnTimer;
            var i = random.Next(enemySpawnPoints.Length);
            spawn(enemySpawnPoints[i].Position);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        bool clicked = Input.IsActionJustPressed("spawn_enemy");
        if (clicked)
        {
            spawn(new Vector3(7, 0, 7));
        }
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
                GetTree().Quit();
    }

    public void spawn(Vector3 pos)
    {
        if (Universe.fight != null)
        {
            var crea = StubFight.spawnStubCreature(pos);
            Universe.fight.creatures.add(crea);
        }
    }

}
