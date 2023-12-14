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
        var model = new SpellModel();
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
        explosionFx.children.add(explosionDmg);
        explosionDmg.children.add(addStatus);
        addStatus.GetProperties<CreateStatusSchema>().children.Add(burningDmg);
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