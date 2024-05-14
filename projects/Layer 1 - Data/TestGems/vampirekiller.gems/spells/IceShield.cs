using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.stats.schemas.skill;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using vampirekiller.eevee.stats.schemas.resources;
using Eevee.vampirekiller.eevee.stats.schemas;
using vampirekiller.logia;
using Util.json;

namespace vampirekiller.gems.spells;

public class IceShield
{

    public SpellModel generateIceShield()
    {
        var spell = Register.Create<SpellModel>();
        spell.entityUid = "spell_ice_shield";
        spell.iconPath = Paths.spells + "iceshield/SpellBook01_59.PNG";
        spell.skins.Add(new()
        {
            animationLibraries = new() { "pro_magic_pack" },
            sourceAnimation = "pro_magic_pack/Standing 1H Magic Attack 01"
        });


        // Base spell FX
        var spellFx = new Statement()
        {
            schema = new SpawnFxSchema()
            {
                scene = Paths.spells + "iceshield/iceshield_spell.tscn"
            }
        };
        spell.statements.add(spellFx);


        // Status creation, no zone + apply only to self
        IStatement createStatusStatement = new Statement()
        {
            targetFilter = TeamFilter.createConditionSelf(),
            schema = getStatusSchema()
        };
        spell.statements.add(createStatusStatement);

        // Status FX 
        var statusFx = new Statement()
        {
            schema = new SpawnFxSchema()
            {
                scene = Paths.spells + "iceshield/iceshield_status.tscn"
            }
        };
        createStatusStatement.statements.add(statusFx);

        return spell;
    }

    private CreateStatusSchema getStatusSchema()
    {
        // Buffs
        var addStatsSchema = new AddStatsSchema();
        addStatsSchema.stats.set(new CreatureBaseLifeRegen() { value = 10 });
        addStatsSchema.stats.set(new ProjectileAddCount() { value = 2 });
        addStatsSchema.stats.set(new ProjectileFireInCircle() { value = true });


        // Status container
        var createStatusSchema = new CreateStatusSchema()
        {
            statusStatements = new List<IStatement>() {
                new Statement() { schema = addStatsSchema }
            }
        };
        createStatusSchema.stats.set(new SkillBaseDuration() { value = 3 });
        createStatusSchema.stats.set(new StatusUnbewitchable() { value = true });
        return createStatusSchema;
    }


    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serializeIceshield()
    {
        var spell = generateIceShield();
        var json = Json.serialize(spell);
        Directory.CreateDirectory("../../../../DB/spells/");
        File.WriteAllText("../../../../DB/spells/" + spell.entityUid + ".json", json);
    }

}
