
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
/// https://www.poewiki.net/wiki/Shock_Nova
/// </summary>
public class ShockNova {
    
    private SpellModel generateShockNova()
    {
        var spell = Register.Create<SpellModel>();
        var fx = new Statement() {
            // Glaceon has to take this effect, spawn a ProjectileNode, keep a ref to the effect, then trigger the children OnCollision
            //  In the case of Shock Nova, we don't need collision as it's not a moving aoe
            schema = new SpawnFxSchema() {
                scene = "res://scenes/db/spells/shockNova.tscm"
            }
        };
        var outerAoe = new Statement() {
            zone = new Zone() {
                zoneType = ZoneType.circle,
                size = new ZoneSize(3),
            },
            targetFilter = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new DamageSchema()
            {
                baseDamage = 10
            }
        };
        var innerRing = new Statement() {
            zone = new Zone() {
                zoneType = ZoneType.circle,
                size = new ZoneSize(3, 0, 1),
            },
            targetFilter = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new DamageSchema()
            {
                baseDamage = 15
            }
        };
        spell.statements.add(fx);
        spell.statements.add(outerAoe);
        spell.statements.add(innerRing);
        // todo add spell.stats (costs, cooldown, maxcharges...)
        // todo add spell.range
        return spell;
    }
    
    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serializeShockNova() {
        var model = generateShockNova();
        // TODO serialize model to json
    }

    [Trait("Category", "ModelTester")]
    [Fact]
    public void testShockNova()
    {
        // ARRANGE
        // load json
        var spellModel = generateShockNova();
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
            raycastPosition = new Vector3(0, 0, 0), // le target sera passed down à tous les enfants de l'action all the way to l'ActionStatementTarget qui va apply le CastSpellScript
            activeItem = item.entityUid,
            fight = fight
        };
        action.applyActionCast();

        // ASSERT
        // todo: check enemies lives
        // todo: check caster's resources got reduced by spell cost
        
    }

}
