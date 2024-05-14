using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Xunit.Abstractions;

namespace vampirekiller.gems.spells;

internal class Meteor : SpellGenerator
{
    public Meteor(ITestOutputHelper output) : base(output)
    {
    }

    protected override SpellModel GenerateSpell()
    {
        var spell = Register.Create<SpellModel>();
        spell.iconPath = "";

        return spell;
    }
}