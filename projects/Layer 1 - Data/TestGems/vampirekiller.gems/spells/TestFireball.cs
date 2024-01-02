
using Eevee.vampirekiller.eevee.stats.schemas;
using Godot;
using Newtonsoft.Json;
using souchy.celebi.eevee.enums;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.stats.schemas.skill;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.util.json;
using vampirekiller.logia.extensions;
using vampirekiller.logia.stub;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.zones;
using Xunit.Abstractions;
using Json = vampirekiller.eevee.util.json.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace vampirekiller.gems.spells;

// Je ne pense pas qu'on ai besoin du SpawnFxSchema dans ce cas pcq 
//      1. On a besoin des enfants dans le ProjectileInstance
//      2. Donc le SpawnProjSchema va créer un ProjectileInstance et l'ajouter au fight
//      3. Donc le GameNode va créer un ProjectileNode en recevant l'event de ProjectileInstance
//      4. Donc on doit déjà avoir la scene au même endroit que les enfants pour créer le ProjectileInstance
// SpawnFxSchema reste utile pour spawn une scène sans logique ni projectile
// SpawnProjectileSchema pourrait inhériter de SpawnFxSchema en quelque sorte
/// <summary>
/// Fireball -> projectile (inst + node) -> explosion (node) -> damage zone -> burn (inst + node)
/// </summary>
public class TestFireball
{
    private readonly ITestOutputHelper output;

    public TestFireball(ITestOutputHelper output)
    {
        this.output = output;
    }

    private SpellModel generateFireballModel()
    {
        var spell = Register.Create<SpellModel>();
        spell.entityUid = "spell_fireball"; // Set un ID constant pour pouvoir load toujours le même

        // Explosion devrait proc sur le onCollision du projectile
        var explosionFx = new Statement() {
            zone = new Zone() { worldOrigin = ZoneOriginType.Target },
            schema = new SpawnFxSchema()
            {
                scene = "res://scenes/db/spells/fireball/fireball_explosion.tscn"
            }
        };
        var explosionDmg = new Statement() {
            zone = new Zone() {
                zoneType = ZoneType.circle,
                size = new ZoneSize(3, 0, 0),
                worldOrigin = ZoneOriginType.Target
            },
            targetFilter = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new DamageSchema() {
                baseDamage = 15
            }
        };
        // Trigger onCollision avec un Enemy
        // Le projectileNode pourra envoyer un TriggerEventOnCollision(t1, t2) au fight.procTriggers(event)
        var collisionListener = new TriggerListener() {
            holderCondition = null,
            triggererCondition = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new TriggerSchemaOnCollision()
        };
        // proc l'fx qui va appliquer ses enfants (dmg)
        explosionFx.triggers.add(collisionListener);
        // important de ne pas mettre fx enfant de dmg dans ce cas-ci, sinon ça réplique l'explosion sur chaque target de la zone et on en veut juste 1 (par projectile)
        explosionFx.statements.add(explosionDmg);
        
        // TODO: il nous faut un listener pour le projectile collision
        //      mais la zone d'explosion peut être calculée sans collision
        // TODO proj avec listener de collision
        // Remember: il peut y avoir plusieurs types de projectiles qui proc onCollision (must collide) ou onArrival (ground-target) ou onExpire (durée/distance)
        var projSchema =  new SpawnProjectileSchema();
        projSchema.children.Add(explosionFx);
        projSchema.scene = "res://scenes/db/spells/fireball/fireball_projectile.tscn";
        projSchema.stats.set(Register.Create<ProjectileAddCount>());
        projSchema.stats.get<ProjectileAddCount>()!.value = 1;
        projSchema.stats.set(new ProjectileBaseSpeed() {
            value = 2
        });
        projSchema.spawnOffset = 0.1f;
        var proj = new Statement()
        {
            zone = new Zone() { worldOrigin = ZoneOriginType.Raycast },
            schema = projSchema
        };
        spell.statements.add(proj);

        // Status --------------------
        // Status va s'appliquer à tous les targets du explosionDmg vu qu'il est son enfant
        var statusSchema = new CreateStatusSchema();
        statusSchema.stats.set(new SkillBaseDuration() { value = 3 });
        statusSchema.stats.set(new SkillMaxDuration() { value = 3 });
        statusSchema.stats.set(new StatusUnbewitchable() { value = true });
        statusSchema.stats.set(new StatusStacks() { value = 1 });
        statusSchema.stats.set(new StatusMaxStacks() { value = 4 });
        IStatement addStatus = new Statement()
        {
            zone = new Zone() { worldOrigin = ZoneOriginType.Target },
            targetFilter = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = statusSchema
        };
        // status doit être enfant de explosion pour l'appliquer à tous les targets dans la zone
        explosionDmg.statements.add(addStatus);

        // OnAdd listener
        var onStatusAddListener = new TriggerListener()
        {
            schema = new TriggerSchemaOnStatusAdd() { spellModelIdFilter = spell.entityUid }
        };

        // Status effects: Burn dot --------------------
        // Créé un timer de 1 second de base activation period
        var timerSchema = new CreateActivationTimerSchema();
        timerSchema.stats.set(new SpellBaseCastTime() { value = 1 });
        var timerStatement = new Statement()
        {
            zone = new Zone() { worldOrigin = ZoneOriginType.Target },
            schema = timerSchema
        };
        timerStatement.triggers.add(onStatusAddListener);
        statusSchema.statusStatements.Add(timerStatement);

        // Apply un damage à chq fois que le timer active.
        var damageOverTime = new Statement()
        {
            zone = new Zone() { worldOrigin = ZoneOriginType.Target },
            schema = new DamageSchema() { //DamageOverTimeSchema() {
                baseDamage = 3
            }
        };
        timerSchema.statements.add(damageOverTime);
        // todo: faut un flag pour trigger à toutes les secondes.
        //      CreateStatus check les flags + crée un timer avec le activationFrequency
        // damage activation listener
        //var activationSchema = new TriggerSchemaOnProcess();
        //activationSchema.stats.set(new SpellBaseCastTime() { value = 1 });
        //damageOverTime.triggers.add(new TriggerListener()
        //{
        //    // BaseCastTime + IncCastTime = TimeBetweenActivations
        //    schema = activationSchema
        //});
        // add the dot to the status schema
        //statusSchema.statusStatements.Add(damageOverTime);

        // Burn FX --------------------
        // ajoute le burn fx en enfant du status, pourrait être l'inverse sans problème
        var burnFx = new Statement() {
            schema = new SpawnFxSchema() {
                scene = "res://scenes/db/spells/fireball/fireball_burn.tscn",
                follow = true
            },
        };
        burnFx.triggers.add(onStatusAddListener);
        // add the fx to the status schema
        statusSchema.statusStatements.Add(burnFx);



        // todo: spell.stats: costs, cooldown, etc
        spell.iconPath = "res://scenes/db/spells/fireball/fireball.png";

        return spell;
    }
    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serializeFireball()
    {
        var spell = generateFireballModel();
        var json = Json.serialize(spell);
        output.WriteLine(json);
        Directory.CreateDirectory("../../../../DB/spells/");
        File.WriteAllText("../../../../DB/spells/" + spell.entityUid + ".json", json);
    }

    [Trait("Category", "ModelTester")]
    [Fact]
    public void testFireball()
    {
        // ARRANGE
        // load json
        var spellModel = generateFireballModel();
        // loadup a mock fight
        Fight fight = new StubFight();
        Diamonds.spellModels.Add(spellModel.entityUid, spellModel);

        // create an item that can cast the spell
        var item = Register.Create<Item>();
        // cast effect
        var cast = new Statement() {
            // FIXME: normalement on a besoin d'un spellInstance qui contient le current cooldown, charges, ...
            schema = new CastSpellSchema() {
                spellModelId = spellModel.entityUid
            }
        };
        // listener with no condition
        var listener = new TriggerListener() {
            schema = new TriggerSchemaOnCastActive()
        };
        cast.triggers.add(listener);
        item.statements.add(cast);

        // add item to player
        var player = fight.creatures.values.First(c => c.creatureGroup == EntityGroupType.Players);
        player.equip(item);
        // add skill to active bar
        player.activeSkills.add(player.allSkills.getAt(0)!);

        // ACT
        // start action
        var action = new ActionCastActive() {
            sourceEntity = player.entityUid,
            raycastPosition = new Vector3(3, 0, 3), // le target sera passed down à tous les enfants de l'action all the way to l'ActionStatementTarget qui va apply le CastSpellScript
            slot = 0,
            fight = fight
        };
        if (action.canApplyCast())
            action.applyActionCast();

        // TODO: Mock ActionCollision qqpart
        new ActionCollision(null, null);

        // ASSERT
        // todo: check enemies lives
        // todo: check caster's resources got reduced by spell cost
        
    }

}
