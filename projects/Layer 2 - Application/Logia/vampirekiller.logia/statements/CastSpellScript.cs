using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.statements;

/// <summary>
/// Not anymore.
/// We have ActionCastActive.applyActionCast
/// The CastSpellSchema should prob still take a spellModelId.
/// But it should create a new ActionCastActive
/// This script is still useful to cast SubSpells triggered from statuses and items
/// So it won't receive an ActionCastActive anymore. It can receive anything.
/// -----
/// Equivalent to Celebi's Actions.castSpell
/// Casts a spell.
/// Must check that the creature actually owns a spell instance corresponding to the asked spell model.
/// Must check that the creature can pay the spell cast costs.
/// Must check that the creature meets the spell cast conditions.
/// </summary>
public class CastSpellScript : IStatementScript
{
    public Type schemaType => typeof(CastSpellSchema);

    public void apply(ActionStatementTarget action)
    {
        //ActionCastActive castAction = action.getParent<ActionCastActive>()!;
        //CreatureInstance caster = castAction.getSourceCreature();
        //CastSpellSchema schema = action.statement.GetProperties<CastSpellSchema>();

        //// get spell
        //SpellInstance? spell = caster.spells.get(s => s.modelUid == schema.spellModelId);
        //if(spell == null)
        //    return;

        //spell.cast(castAction);
    }
}
