using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.zones;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using vampirekiller.eevee.conditions.schemas;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using souchy.celebi.eevee.enums;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.vampirekiller.eevee;
using vampirekiller.logia.stub;
using vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.enums;
using vampirekiller.logia.extensions;
using vampirekiller.eevee.actions;

namespace vampirekiller.gems.spells;

public class Swipe
{
    private SpellModel generate()
    {
        var spell = Register.Create<SpellModel>();
        spell.entityUid = "spell_swipe";

        var swipeAoe = new Statement() {
            zone = new Zone() {
                zoneType = ZoneType.circleHalf,
                size = new ZoneSize(2),
            },
            targetFilter = new Condition() {
                schema = new TeamFilter() {
                    team = TeamRelationType.Enemy
                }
            },
            schema = new DamageSchema() {
                baseDamage = 5
            }
        };

        spell.statements.add(swipeAoe);
        return spell;
    }

    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serialize()
    {
        var spell = generate();
        var json = Json.serialize(spell);
        File.WriteAllText("../../../../DB/spells/" + spell.entityUid + ".json", json);
    }

    //[Trait("Category", "ModelTester")]
    //[Fact]
    //public void test() {

    //    // ARRANGE
    //    var spellModel = generate();
    //    Fight fight = new StubFight();
    //    Diamonds.spells.Add(spellModel.entityUid, spellModel);

    //    var item = Register.Create<Item>();
    //    var cast = new Statement() {
    //        schema = new CastSpellSchema() {
    //            spellModelId = spellModel.entityUid
    //        }
    //    };
    //    var listener = new TriggerListener() {
    //        schema = new TriggerSchemaOnCastActive()
    //    };
    //    cast.triggers.add(listener);
    //    item.statements.add(cast);

    //    var player = fight.creatures.values.First(c => c.creatureGroup == EntityGroupType.Players);
    //    player.equip(item);
    //    player.activeSkills.add(player.allSkills.getAt(0)!);

    //    // ACT
    //    var action = new ActionCastActive()
    //    {
    //        sourceEntity = player.entityUid,
    //        raycastPosition = new Godot.Vector3(0, 0, 0),
    //        slot = 0,
    //        fight = fight
    //    };
    //    action.applyActionCast();

    //    // ASSERT

    //}
}