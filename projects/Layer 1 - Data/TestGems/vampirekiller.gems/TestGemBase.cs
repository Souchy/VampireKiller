using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.util.json;

namespace vampirekiller.gems;

public abstract class TestGemBase<T> where T : Identifiable
{
    private static string DB_PATH_PREFIX = "../../../../DB";

    public enum TestGemBaseType {
        Spells,
        Creatures,
    }

    public abstract string getName();
    public abstract TestGemBaseType getType();
    public abstract T generateImpl(T model);
    protected abstract TestGemFight testArrange();
    protected abstract void testAct(TestGemFight fight);
    protected abstract void testAssert(TestGemFight fight);
    
    public T generate()
    {
        T model = Register.Create<T>();
        model.entityUid = this.getName();
        return this.generateImpl(model);
    }

    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void serialize() {
        var model = this.generate();
        var json = Json.serialize(model);
        File.WriteAllText(resolveDBJsonPath(this.getType(), this.getName()), json);
    }

    [Trait("Category", "ModelTester")]
    [Fact]
    public void test()
    {
        TestGemFight fight = testArrange();
        testAct(fight);
        testAssert(fight);
    }

    private static string resolveDBJsonPath(TestGemBaseType type, string name)
    {
        return String.Format(
            "{0}/{1}/{2}.json",
            DB_PATH_PREFIX,
            type.ToString().ToLower(),
            name
        );
    }
}
