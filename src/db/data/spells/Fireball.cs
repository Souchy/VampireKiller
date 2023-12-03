namespace VampireKiller.db;

public class FireballSpell : SpellModel
{

    public FireballSpell()
    {
        this.statements.Add(new Damage()
        {
            damage = 120
        });
    }

}

public class FireballProjectile
{

}
