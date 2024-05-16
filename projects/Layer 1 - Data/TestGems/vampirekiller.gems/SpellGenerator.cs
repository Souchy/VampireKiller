using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.json;
using vampirekiller.eevee.util.json;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Xunit.Abstractions;

namespace vampirekiller.gems;

public abstract class SpellGenerator
{
    protected readonly ITestOutputHelper output;
    public SpellGenerator(ITestOutputHelper output)
    {
        this.output = output;
    }

    protected abstract SpellModel GenerateSpell();
    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void SerializeSpell()
    {
        var spell = GenerateSpell();
        var json = Json.serialize(spell);
        output.WriteLine(json);
        Directory.CreateDirectory("../../../../DB/spells/");
        File.WriteAllText("../../../../DB/spells/" + spell.entityUid + ".json", json);
    }

}