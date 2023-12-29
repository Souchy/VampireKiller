using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        spell.applyStatementContainer(action);
    }

}