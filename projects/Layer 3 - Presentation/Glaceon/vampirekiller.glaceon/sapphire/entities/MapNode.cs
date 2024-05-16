using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using souchyutil.godot.animation;
using SouchyUtil.Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.util;
using vampirekiller.glaceon.util;
using vampirekiller.logia;
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
        if (StubFight.enemyModel == null)
            StubFight.initEnemyModel(); // TODO modelId
        enemySpawnPoints = this.GetNodesOfType<Marker3D>().Where(c => c.IsInGroup(EnemySpawnPointGroup)).ToArray();
    }

    public override void _Process(double delta)
    {
        //spawnTimerDelta -= delta;
        //if (spawnTimerDelta <= 0)
        //{
        //    spawnTimerDelta = spawnTimer;
        //    var i = random.Next(enemySpawnPoints.Length);
        //    spawn(enemySpawnPoints[i].Position);
        //}
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        bool clicked = Input.IsActionJustPressed("spawn_enemy");
        if (clicked)
        {
            spawn(new Vector3(7, 0, 7), StubFight.enemyModel.entityUid);
        }
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
                GetTree().Quit();
    }

    public void spawn(Vector3 pos, ID modelId)
    {
        if (Universe.fight != null)
        {
            CrowdInstance crowd = Universe.fight.crowds.get(modelId); //StubFight.enemyModel.entityUid); //crea.model.entityUid);
            if (crowd?.Instances.size() >= 100)
                return;

            int packSize = 10;
            int width = (int) Math.Sqrt(packSize);
            int height = packSize / width;
            for (int i = 0; i < packSize; i++)
            {
                int x = i % width - width / 2;
                int z = i / width - width / 2;
                Vector3 creaPos = new(pos.X + x, pos.Y, pos.Z + z);

                var crea = StubFight.spawnStubCreature(creaPos);
                if (crowd == null)
                {
                    crowd = Register.Create<CrowdInstance>();
                    Universe.fight.crowds.add(modelId, crowd);

                    var node = crowd.get<CrowdNode>();
                    // Set Mesh
                    var scene = AssetCache.Load<PackedScene>(crea.currentSkin.scenePath, ".tscn", ".glb", ".gltf").Instantiate<Node3D>();
                    var meshInstance = scene.GetFirstChildOfType<MeshInstance3D>();
                    var mesh = meshInstance.Mesh;
                    node.SetMesh(mesh);

                    // Set animation data
                    var data = AssetCache.LoadBakedAnimationData(crea.currentSkin.animationLibraries[0]);
                    node.SetAnimationLibrary(data);
                }
                if (crowd.Instances.size() < 100)
                    crowd.Instances.add(crea);
            }
            //Universe.fight.creatures.add(crea);
        }
    }

}
