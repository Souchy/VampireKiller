using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.vampirekiller.eevee.spells;

namespace vampirekiller.logia.extensions;

public static class SpellExtensions
{
    public static void cast(this SpellInstance spell, ActionCastActive action)
    {
        // TODO: 
        // check spell costs against creature's resources
        // check spell cast conditions

        // apply
        // TODO: update les ressources du player, update le nombre de charges dans le spellinstance, update le cooldown, 
        // l'action devrait déjà contenir le raycast mousetarget depuis la root action
        spell.getModel().applyStatementContainer(action);
    }

    public static SpellInstance createInstance(this SpellModel model)
    {
        var spell = Register.Create<SpellInstance>();
        spell.modelUid = model.entityUid;
        spell.skin = model.skins[0];
        //foreach(var s in model.statements.values)
        //{
        //    spell.statements.add(s.copy());
        //}
        foreach(var s in model.stats.values)
        {
            spell.stats.set(s.copy());
        }
        return spell;
    }

}