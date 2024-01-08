using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.triggers;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.gems.spells;

public abstract class TestGemBaseSpellModel : TestGemBase<SpellModel>
{
    public override TestGemBaseType getType()
    {
        return TestGemBaseType.Spells;
    }

    protected override TestGemFight testArrange()
    {
        SpellModel spell = this.generate();
        Diamonds.spellModels.Add(spell.entityUid, spell);
        
        CreatureInstance player = TestGemFight.spawnStubCreature(true);
        Item castItem = spawnTestCastItem(spell);
        player.equip(castItem);

        TestGemFight fight = new TestGemFight();
        fight.creatures.add(player);
        spawnTestTargets(fight);

        return fight;
    }

    protected override void testAct(TestGemFight fight)
    {
        CreatureInstance player = fight.getFirstCreature(true);
        CreatureInstance target = fight.getFirstCreature(false);
        var action = new ActionCastActive()
        {
            sourceEntity = player.entityUid,
            raycastPosition = target.position, // le target sera passed down à tous les enfants de l'action all the way to l'ActionStatementTarget qui va apply le CastSpellScript
            slot = 0,
            fight = fight
        };
        if (action.canApplyCast())
            action.applyActionCast();
    }

    protected void spawnTestTargets(Fight fight)
    {
        fight.creatures.add(
            TestGemFight.spawnStubCreature(false, new Vector3(1, 0, 0))
        );
    }

    protected Item spawnTestCastItem(SpellModel spellToCast)
    {
        var item = Register.Create<Item>();
        // cast effect
        var cast = new Statement()
        {
            // FIXME: normalement on a besoin d'un spellInstance qui contient le current cooldown, charges, ...
            schema = new CastSpellSchema()
            {
                spellModelId = spellToCast.entityUid
            }
        };
        // listener with no condition
        var listener = new TriggerListener()
        {
            schema = new TriggerSchemaOnCastActive()
        };
        cast.triggers.add(listener);
        item.statements.add(cast);
        return item;
    }
}