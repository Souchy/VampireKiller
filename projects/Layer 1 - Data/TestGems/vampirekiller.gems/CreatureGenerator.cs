using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.json;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Xunit.Abstractions;

namespace vampirekiller.gems;

public abstract class CreatureGenerator
{
    protected readonly ITestOutputHelper output;
    public CreatureGenerator(ITestOutputHelper output)
    {
        this.output = output;
    }

    protected abstract CreatureModel GenerateCreature();
    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void SerializeCreature()
    {
        var model = GenerateCreature();
        var json = Json.serialize(model);
        output.WriteLine(json);
        Directory.CreateDirectory("../../../../DB/creatures/");
        File.WriteAllText("../../../../DB/creatures/" + model.entityUid + ".json", json);
    }

}