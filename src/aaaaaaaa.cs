using System.Collections.Generic;
using EffectStats;
using Godot;
using ProjectileStats;
using VampireKiller;

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


partial class Effect : Node
{
    public CreatureInstance caster;
    public Vector3 velocity;
    public List<Statement> statements = new();
    // 3d model / vfx
    // duration..?
    // colision hitbox aoe, or null if single target

    // onReady() -> foreach statements -> filter trigger onReady 
    // onProcess() -> foreach statements -> filter trigger onTick 
    // onHit() -> foreach statements -> filter trigger onHit -> apply -> damage.apply + spawnEffect.apply
    // onRemove/Expire() -> foreach statements -> filter trigger onExpire
    public void onHit(CreatureInstance target)
    {
        foreach (var statement in statements)
        {
            if (statement.triggers.Contains(Trigger.OnHit))
            {
                new ActionEffectTarget(null, new(), statement, target);
                statement.apply(null);
            }
        }
    }
}

partial class Projectile : Effect
{

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
    public int effectCount;
    // public Effect effect;
    public PackedScene effectscene;
    public int initialSpeed;
    public override void apply(ActionEffectTarget action)
    {
        // action.caster.addScene(effect);
        Vector3 destinationVector = action.target.Position - action.caster.Position;

        var radiusBoost = action.caster.getTotalStats().get<EffectRadius>();

        for(int i = 0; i < effectCount; i++) {
            Effect effect = effectscene.Instantiate<Effect>();
            effect.caster = action.caster;
            effect.velocity = destinationVector *= initialSpeed; // FIXME
            // effect.collisionBox *= radiusBoost.value; // TODO
        }

    }
}

class SpawnProjectile : Statement
{
    public int projectileCount;
    public PackedScene effectscene;
    public int initialSpeed;
    public override void apply(ActionEffectTarget action)
    {
        Vector3 destinationVector = action.target.Position - action.caster.Position;

        var addproj = action.caster.getTotalStats().get<ProjectileAdditional>();
        action.caster.getTotalStats().get<ProjectilePierce>();
        action.caster.getTotalStats().get<ProjectileSpeed>();
        action.caster.getTotalStats().get<ProjectileChain>();
        action.caster.getTotalStats().get<ProjectileRadius>();
        action.caster.getTotalStats().get<ProjectileFork>();

        int numberOfProj = projectileCount + addproj.value;
        for (int i = 0; i < numberOfProj; i++)
        {
            // Node node = effect.Duplicate();
            Projectile effect = effectscene.Instantiate<Projectile>();
            effect.caster = action.caster;
            effect.velocity = destinationVector *= initialSpeed; // FIXME

            action.caster.AddChild(effect); // action.caster.addScene(effect);
        }
    }
}


//////////////////////// Implementation
class FireballSpellModel : SpellModel
{
    private PackedScene projectileScene = GD.Load<PackedScene>("res://fireball_projectile.tscn");
    private PackedScene explosionScene = GD.Load<PackedScene>("res://fireball_explosion.tscn");
    public FireballSpellModel()
    {
        var projectile = new SpawnProjectile() { 
            effectscene = projectileScene,
            initialSpeed = 10 
        };
        var explosion = new SpawnEffect() { 
            effectscene = explosionScene
        };
        var dmg = new Damage() { damage = 100 };

        // for each cast, trigger proj
        this.statements.Add(projectile);
        // for each proj, trigger explosion
        explosion.triggers.Add(Trigger.OnHit);
        projectile.children.Add(explosion);
        // for each explosion, trigger dmg
        dmg.triggers.Add(Trigger.OnHit);
        explosion.children.Add(dmg);
    }
}
/// <summary>
/// Scene: vfx + hitbox 
/// </summary>
partial class FireballProjectile : Projectile
{
    public FireballProjectile()
    {
    }
}
/// <summary>
/// Scene: vfx + hitbox
/// </summary>
partial class FireballExplosion : Effect
{
    public FireballExplosion()
    {
    }
}
