
using Godot;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using souchy.celebi.eevee.enums;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
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
using Json = vampirekiller.eevee.util.json.Json;

namespace vampirekiller.gems.spells;

/// <summary>
/// https://www.poewiki.net/wiki/Shock_Nova
/// </summary>
public class ShockNova {
    
    private SpellModel generateShockNova()
    {
        // IMPORTANT: le Register donne toujours un nouvel ID. Donc on doit utiliser un autre id pour les répertorier
        // IMPORTANT: Solution plus évidente: utilise le nom du spell 
        var spell = Register.Create<SpellModel>();
        spell.entityUid = "spell_shock_nova"; // Set un ID constant pour pouvoir load toujours le même

        var fx = new Statement() {
            // Glaceon has to take this effect, spawn a ProjectileNode, keep a ref to the effect, then trigger the children OnCollision
            //  In the case of Shock Nova, we don't need collision as it's not a moving aoe
            schema = new SpawnFxSchema() {
                scene = "res://scenes/db/spells/shockNova.tscn"
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
        var spell = generateShockNova();
        var json = Json.serialize(spell);
        Directory.CreateDirectory("../../../../DB/spells/");
        File.WriteAllText("../../../../DB/spells/" + spell.entityUid + ".json", json);
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
        Diamonds.spellModels.Add(spellModel.entityUid, spellModel);

        // create an item that can cast the spell
        var item = Register.Create<Item>();
        // cast effect
        var castStatement = new Statement() {
            schema = new CastSpellSchema() {
                spellModelId = spellModel.entityUid
            }
        };
        var listener = new TriggerListener() {
            schema = new TriggerSchemaOnCastActive()
        };
        castStatement.triggers.add(listener);
        item.statements.add(castStatement);

        // add item to player & learn a new spell instance
        var player = fight.creatures.values.First(c => c.creatureGroup == EntityGroupType.Players);
        player.equip(item);
        // add skill to active bar
        player.activeSkills.add(player.allSkills.getAt(0)!);

        // ACT
        // start action
        var action = new ActionCastActive() {
            sourceEntity = player.entityUid,
            raycastPosition = new Vector3(0, 0, 0), // le target sera passed down à tous les enfants de l'action all the way to l'ActionStatementTarget qui va apply le CastSpellScript
            slot = 0, //item.entityUid,
            fight = fight
        };
        action.applyActionCast();

        // ASSERT
        // todo: check enemies lives
        // todo: check caster's resources got reduced by spell cost
        
    }

}
