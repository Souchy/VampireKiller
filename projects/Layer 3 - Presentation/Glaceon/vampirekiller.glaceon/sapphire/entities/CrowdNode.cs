using Godot;
using Godot.Sharp.Extras;
using souchyutil.godot.components.multimesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.enums;
using vampirekiller.glaceon.util;
using vampirekiller.logia;
using VampireKiller.eevee.creature;

namespace vampirekiller.glaceon.sapphire.entities;

public partial class CrowdNode : Crowd
{
    #region Nodes
    [NodePath]
    public Node3D CreatureNodes { get; set; }
    #endregion

    #region Properties
    public CrowdInstance CrowdInstance { get; set; }
    #endregion

    public void init(CrowdInstance instance)
    {
        CrowdInstance = instance;
        CrowdInstance.set<CrowdNode>(this);
        CrowdInstance.Instances.GetEntityBus().subscribe(this);
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    #region EventHandlers
    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void OnAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = AssetCache.Load<PackedScene>(Paths.entities + "EnemyNode.tscn").Instantiate<EnemyNode>();
        node.init(inst);
        inst.set(this);
        CreatureNodes.AddChild(node);

        node.Model.Visible = false;
        node.Model.ProcessMode = ProcessModeEnum.Disabled;
        node.NavigationAgent3D.ProcessMode = ProcessModeEnum.Disabled;

        EnemyNode enemy = (EnemyNode) node;
        var data = new InstanceData();
        enemy.InstanceData = data;

        base.AddInstances(data);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void OnRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = inst.get<CreatureNode>();
        EnemyNode enemy = (EnemyNode) node;
        base.RemoveInstances(enemy.InstanceData);

        // GD.Print("Game on remove crowd creature: " + node);
        node.QueueFree();
    }
    #endregion

}
