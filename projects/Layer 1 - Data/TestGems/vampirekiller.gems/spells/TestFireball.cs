
using Godot;
using souchy.celebi.eevee.enums;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;
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


namespace vampirekiller.gems.spells;

/// <summary>
/// 
/// </summary>
public class TestFireball
{
    private SpellModel generateFireballModel()
    {
        var spell = Register.Create<SpellModel>();
        var projectileFx = new Statement() {
            // Glaceon has to take this effect, spawn a ProjectileNode, keep a ref to the effect, then trigger the children OnCollision
            schema = new SpawnFxSchema() {
                scene = "res://scenes/db/spells/fireball/fireball_projectile.tscn"
            }
        };
        //projectileFx.triggers.Add(TriggerType.OnCollision);
        var explosionFx = new Statement() {
            schema = new SpawnFxSchema() {
                scene = "res://scenes/db/spells/fireball/fireball_explosion.tscn"
            }
        };
        var explosionDmg = new Statement() {
            schema = new DamageSchema() {
                baseDamage = 15
            }
        };
        IStatement addStatus = new Statement() {
            //schema = new CreateStatusSchema() {
            // 
            //}
        };
        var burningDmg = new Statement() {
            schema = new DamageSchema() {
                baseDamage = 3
            }
        };

        // TODO: il nous faut un listener pour le projectile collision
        //      mais la zone d'explosion peut être calculée sans collision

        spell.statements.add(explosionFx);
        explosionFx.statements.add(explosionDmg);
        explosionDmg.statements.add(addStatus);
        addStatus.GetProperties<CreateStatusSchema>().statusStatements.Add(burningDmg);
        return spell;
    }
    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serializeFireball()
    {
        var spell = generateFireballModel();
        // todo serialize to json
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
        Diamonds.spells.Add(spellModel.entityUid, spellModel);
        // create an item that can cast the spell
        var item = Register.Create<Item>();
        // cast effect
        var cast = new Statement() {
            // FIXME: normalement on a besoin d'un spellInstance qui contient le current cooldown, charges, ...
            schema = new CastSpellSchema() {
                spellId = spellModel.entityUid
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
        player.inventory.items.add(item);
        player.inventory.activeSlots.add(item);

        // ACT
        // start action
        var action = new ActionCastActive() {
            sourceCreature = player.entityUid,
            raycastPosition = new Vector3(3, 0, 3), // le target sera passed down à tous les enfants de l'action all the way to l'ActionStatementTarget qui va apply le CastSpellScript
            activeItem = item.entityUid,
            fight = fight
        };
        action.applyActionCast();

        // ASSERT
        // todo: check enemies lives
        // todo: check caster's resources got reduced by spell cost
        
    }

}
