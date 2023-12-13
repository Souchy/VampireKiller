using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using Util.structures;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;

namespace Glaceon.scenes.game;
public class SceneManager
{
    public static readonly SceneManager instance = new SceneManager();
    public SmartSet<CreatureNode> creatures { get; init; } = SmartSet<CreatureNode>.Create<CreatureNode>();
    //public SmartSet<ProjectileNode> projectiles { get; init; } = SmartSet<ProjectileNode>.Create<ProjectileNode>();
    private SceneManager() {}

    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void onAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<CreatureNode>();
        inst.GetEntityBus().subscribe(node);
        creatures.add(node);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {

    }

    [Subscribe(nameof(SmartSet<Projectile>.add))]
    public void onAddProjectile(SmartSet<Projectile> list, Projectile inst)
    {

    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<Projectile> list, Projectile inst)
    {

    }

}
