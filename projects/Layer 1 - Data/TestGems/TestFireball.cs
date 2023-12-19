using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;

namespace TestGems;

public class TestFireball
{

    [Trait("Category", "ModelGenerator")]
    [Fact]
    public void generateFireballModel()
    {
        var model = Register.Create<SpellModel>(); //new SpellModel();
        var explosionFx = new Statement()
        {
            //zone = new Zone() { 
            //    type = ZoneType.circle
            //    radius = 3
            //},
            //schema = new SpawnFxSchema()  // Glaceon has to take this effect, spawn a ProjectileNode, keep a ref to the effect, then trigger the children OnCollision
        };
        var explosionDmg = new Statement()
        {
            schema = new DamageSchema()
            {
                baseDamage = 15
            }
        };
        //explosionDmg.triggers.Add(TriggerType.OnCollision);
        IStatement addStatus = new Statement()
        {
            //schema = new CreateStatusSchema() {
            // 
            //}
        };
        var burningDmg = new Statement()
        {
            schema = new DamageSchema()
            {
                baseDamage = 3
            }
        };
        model.statements.add(explosionFx);
        explosionFx.statements.add(explosionDmg);
        explosionDmg.statements.add(addStatus);
        addStatus.GetProperties<CreateStatusSchema>().statusStatements.Add(burningDmg);
        // TODO serialize fireball to json file
    }

    [Trait("Category", "ModelTester")]
    [Fact]
    public void testFireball()
    {
        // load json
        // loadup a mock fight
        // test the effects
    }

}
