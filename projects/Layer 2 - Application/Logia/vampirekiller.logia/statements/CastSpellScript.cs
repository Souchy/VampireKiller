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
        ActionCastActive castAction = action.getParent<ActionCastActive>()!;
        CreatureInstance caster = castAction.getSourceCreature();
        CastSpellSchema schema = action.statement.GetProperties<CastSpellSchema>();

        // get spell
        SpellInstance? spell = caster.spells.get(s => s.modelUid == schema.spellModelId);
        if(spell == null)
            return;

        // TODO: 
        // check spell costs against creature's resources
        // check spell cast conditions

        // apply
        // TODO: update les ressources du player, update le nombre de charges dans le spellinstance, update le cooldown, 
        // l'action devrait déjà contenir le raycast mousetarget depuis la root action
        spell.applyStatementContainer(action);
    }
}
