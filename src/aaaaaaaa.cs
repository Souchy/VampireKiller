using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using EffectStats;
using Godot;
using ProjectileStats;
using VampireKiller;

namespace aaaaaaaa;

// ------------------------------------------------------------------------------------------------------ Code
class Item
{
    public object icon;
    public Node3D model3d;
    public int quantity = 0;
    public List<Statement> statements;
}

interface Active
{
    public void cast();
}

class Weapon : Item, Active
{
    public int ammo;

    public void cast()
    {
        // spawn Projectile Effects
    }
}

class SpellScroll : Item, Active
{
    public SpellModel spell;
    public int cooldown;
    public int charges;

    public void cast()
    {
        // spawn Aoe Effect
    }
}

// ------------------------------------------------------------------------------------------------------ World Effects

public interface IEffect
{
    public CreatureInstance caster { get; set; }
    public Vector3 velocity { get; set; }
    public long activationFrequency { get; set; }
    public long duration { get; set; }
    public List<Statement> statements { get; set; }
}
partial class Effect : Node, IEffect
{
    public CreatureInstance caster { get; set; }
    public Vector3 velocity { get; set; }
    public long activationFrequency { get; set; }
    public long duration { get; set; }
    public List<Statement> statements { get; set; } = new();
    // 3d model / vfx
    // duration..?
    // colision hitbox aoe, or null if single target

    // onReady() -> foreach statements -> filter trigger onReady 
    // onProcess() -> foreach statements -> filter trigger onTick 
    // onHit() -> foreach statements -> filter trigger onHit -> apply -> damage.apply + spawnEffect.apply
    // onRemove/Expire() -> foreach statements -> filter trigger onExpire
    public void onCollisionDetected(CreatureInstance target)
    {
        foreach (var statement in statements)
        {
            if (statement.triggers.Contains(Trigger.OnHit))
            {
                var action = new ActionEffectTarget(caster, target.Position, statement, target);
                statement.apply(action);
            }
        }
    }
    public void onProcess() {
        // if frequency at the right time...
        if(true) {
            onActivation();
        }
    }
    public virtual void onActivation() {

    }
}

partial class StatusEffect : Effect
{
    public CreatureInstance holder;
    public override void onActivation()
    {
        base.onActivation();
        foreach (var statement in statements)
        {
            if (statement.triggers.Contains(Trigger.OnProcess))
            {
                var action = new ActionEffectTarget(caster, holder.Position, statement, holder);
                statement.apply(action);
            }
        }
    }
}
partial class ProjectileEffect : Effect
{
}

// ------------------------------------------------------------------------------------------------------ Statements
class Diamonds {
    public static Dictionary<string, PackedScene> scenes = new();
}
/// <summary>
///  how do we change the number or speed of projectiles ? 
///  May need multiple implementations:
///     SpawnProjectile
///  or even individual:
///     SpawnFireballProjectile
///     SpawnFireballExplosion
/// </summary>
class SpawnEffect : Statement
{
    public int spawnCount;
    
    /// <summary>
    /// Scene Name
    /// </summary>
    public string effectSceneName;
    // public PackedScene effectscene;
    /// <summary>
    /// Effect speed, like for projectiles or even areas, some effects go faster
    /// </summary>
    public int initialVelocity;
    /// <summary>
    /// how many activations per second (some effects activate multiple times, like statuses, or multi-hit things)
    /// </summary>
    public int frequency;
    /// <summary>
    /// how many seconds the effect should last
    /// </summary>
    public int duration;
    /// <summary>
    /// Get the PackedScene
    /// </summary>
    public override void apply(ActionEffectTarget action)
    {
        var effectScene = this.GetScene();

        Vector3 destinationVector = action.target.Position - action.caster.Position;
        var radiusBoost = action.caster.getTotalStats().get<EffectRadius>();

        for(int i = 0; i < spawnCount; i++) {
            Effect effect = effectScene.Instantiate<Effect>();
            effect.caster = action.caster;
            effect.velocity = destinationVector *= initialVelocity; // FIXME
            // effect.collisionBox *= radiusBoost.value; // TODO
            
            // Add to scene tree
            action.caster.AddChild(effect);
        }
    }
}

/// <summary>
/// this is interesting,
/// but then how do you simulate the game headless? -> on the server, or in unit tests
/// i need those effects
/// </summary>
// class ClientSideSpawnSceneCommandHandler {
//     public void onRequestSpawnScene(ActionEffectTarget action) 
//     {
//         var s = (SpawnEffect) action.statement;
//         var effectScene = s.GetScene();

//         Vector3 destinationVector = action.target.Position - action.caster.Position;
//         var radiusBoost = action.caster.getTotalStats().get<EffectRadius>();

//         for(int i = 0; i < s.spawnCount; i++) {
//             Effect effect = effectScene.Instantiate<Effect>();
//             effect.caster = action.caster;
//             effect.velocity = destinationVector *= s.initialVelocity; // FIXME
//             // effect.collisionBox *= radiusBoost.value; // TODO
            
//             // Add to scene tree
//             action.caster.AddChild(effect);
//         }
//     }
// }
static class ClientSideSpawnEffectExtension {
    public static PackedScene GetScene(this SpawnEffect spawnEffect) => Diamonds.scenes[spawnEffect.effectSceneName];
}

class SpawnStatusEffect : SpawnEffect
{
    public override void apply(ActionEffectTarget action)
    {
        var effectScene = this.GetScene();
        StatusEffect status = effectScene.Instantiate<StatusEffect>();
        // Add to scene tree
        action.caster.AddChild(status);
    }
}
class SpawnProjectileEffect : SpawnEffect
{
    public override void apply(ActionEffectTarget action)
    {
        var effectScene = this.GetScene();
        Vector3 destinationVector = action.target.Position - action.caster.Position;

        var radiusBoost = action.caster.getTotalStats().get<EffectRadius>();
        var addproj = action.caster.getTotalStats().get<ProjectileAdditional>();
        action.caster.getTotalStats().get<ProjectilePierce>();
        action.caster.getTotalStats().get<ProjectileSpeed>();
        action.caster.getTotalStats().get<ProjectileChain>();
        action.caster.getTotalStats().get<ProjectileRadius>();
        action.caster.getTotalStats().get<ProjectileFork>();

        int numberOfProj = spawnCount + addproj.value;
        for (int i = 0; i < numberOfProj; i++)
        {
            // Node node = effect.Duplicate();
            ProjectileEffect proj = effectScene.Instantiate<ProjectileEffect>();
            proj.caster = action.caster;
            proj.velocity = destinationVector *= initialVelocity; // FIXME
            // TODO: proj.collisionBox *= radiusBoost.value;
            // TODO: proj.collisionMask = enemies // action.caster.team.inverse()

            // Add to scene tree
            action.caster.AddChild(proj);
        }
    }
}


// ------------------------------------------------------------------------------------------------------ Fireball implementation (statements + effects)
/// <summary>
/// CODE ONLY
/// </summary>
class FireballSpellModel : SpellModel
{
    // private PackedScene projectileScene = GD.Load<PackedScene>("res://fireball_projectile.tscn");
    // private PackedScene explosionScene = GD.Load<PackedScene>("res://fireball_explosion.tscn");
    // private PackedScene burningStatusScene = GD.Load<PackedScene>("res://fireball_burning.tscn");
    /// <summary>
    /// Setup statements
    /// </summary>
    public FireballSpellModel()
    {
        var projectile = new SpawnProjectileEffect() { 
            effectSceneName = "res://fireball_projectile.tscn",
            // effectscene = projectileScene,
            initialVelocity = 10 
        };
        var explosion = new SpawnEffect() { 
            effectSceneName = "res://fireball_explosion.tscn",
            // effectscene = explosionScene
        };
        var burningStatus = new SpawnStatusEffect() { 
            effectSceneName = "res://fireball_burning.tscn",
            // effectscene = burningStatusScene,
            duration = 3,
            frequency = 1,
        };
        
        var dmg = new Damage() { damage = 100 };
        dmg.triggers.Add(Trigger.OnHit);
        var dot = new Damage() { damage = 1 };
        dot.triggers.Add(Trigger.OnProcess);

        // for each cast, trigger proj
        this.statements.Add(projectile);
        // for each proj, trigger explosion
        explosion.triggers.Add(Trigger.OnHit);
        projectile.children.Add(explosion);
        // for each explosion, trigger dmg
        explosion.children.Add(dmg);
        explosion.children.Add(burningStatus);
        burningStatus.children.Add(dot);
    }
}

// ----------------------------------------------------------------------------- Glaceon side
/// <summary>
/// PARTIAL SCENE: vfx + hitbox. Dont need these classes unless you want additional code to control the vfx
/// </summary>
partial class FireballProjectile : ProjectileEffect
{
    public FireballProjectile()
    {
    }
}
/// <summary>
/// PARTIAL SCENE: vfx + hitbox
/// </summary>
partial class FireballExplosion : Effect
{
    public FireballExplosion()
    {
    }
}
/// <summary>
/// PARTIAL SCENE: vfx 
/// </summary>
partial class FireballStatusBurning : StatusEffect
{
    public FireballStatusBurning()
    {
        // this.activationFrequency = 10;
    }
}

