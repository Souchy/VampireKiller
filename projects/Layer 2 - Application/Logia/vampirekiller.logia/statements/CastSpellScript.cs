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
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.statements;

public class CastSpellScript : IStatementScript
{
    public Type schemaType => typeof(CastSpellSchema);

    public void apply(ActionStatementTarget action)
    {
        ActionCastActive castAction = action.getParent<ActionCastActive>()!;
        IStatement statement = action.statement;
        CastSpellSchema schema = statement.GetProperties<CastSpellSchema>();
        // get spell
        // FIXME: should be a spell instance
        var spellModel = Diamonds.spells[schema.spellId];
        // apply
        // l'action devrait déjà contenir le raycast mousetarget depuis la root action
        spellModel.applyStatementContainer(action);
    }
}
