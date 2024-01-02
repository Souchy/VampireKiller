using Eevee.vampirekiller.eevee.stats.schemas;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.enums;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.stub;

public class StubFight : Fight
{
    private static String[] creatureMeshSceneVariantNames = new String[] { "Ork", "Golem", "Slayer", "Dwarf", "Pig" };

    public StubFight()
    {
        var player = spawnStubPlayer();
        creatures.add(player);
        // entities.add(player);

        for (int i = 0; i < 10; i++)
        {
            int radius = 14;
            double deg = (300.0 / 10.0) * (Math.PI / 180.0);
            var z = (float) Math.Sin(i * deg) * radius;
            var x = (float) Math.Cos(i * deg) * radius;
            var enemy = spawnStubCreature(new(x, 0, z));
            creatures.add(enemy);
            // entities.add(enemy);
        }
    }

    public static CreatureInstance spawnStubPlayer()
    {
        var creaModel = Register.Create<CreatureModel>();
        creaModel.meshScenePath = "res://scenes/db/creatures/PFCCharacter.tscn";
        creaModel.meshSceneVariant = "Sorcerer";
        creaModel.iconPath = "res://icon.svg";
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 200;
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 200;
        creaModel.baseStats.set(Register.Create<ProjectileAddCount>());
        creaModel.baseStats.get<ProjectileAddCount>()!.value = 2;
        creaModel.baseStats.set(new ProjectileIncreasedSpeed()
        {
            value = 500
        });

        var crea = Register.Create<CreatureInstance>();
        crea.model = creaModel;
        crea.spawnPosition = new Vector3(0.1f, 0, 0); // pas sur pourquoi ça bug si on déplace pas le player au spawn
        crea.creatureGroup = EntityGroupType.Players;
        crea.set<Team>(Team.A);

        var fireball = Register.Create<SpellInstance>();
        fireball.modelUid = Diamonds.spells["spell_fireball"].entityUid;
        crea.activeSkills.add(fireball);

        var shocknova = Register.Create<SpellInstance>();
        shocknova.modelUid = Diamonds.spells["spell_shock_nova"].entityUid;
        crea.activeSkills.add(shocknova);

        return crea;
    }

    public static CreatureInstance spawnStubCreature(Vector3 vec)
    {
        var creaModel = Register.Create<EnemyModel>();
        creaModel.meshScenePath = "res://scenes/db/creatures/PFRCharacterBR.tscn";
        creaModel.meshSceneVariant = getRandomCreatureMeshSceneVariant();
        creaModel.iconPath = "res://icon.svg";
        creaModel.ai = new AiMelee();
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 2;
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 2;

        var crea = Register.Create<CreatureInstance>();
        crea.model = creaModel;
        crea.spawnPosition = vec;
        crea.creatureGroup = EntityGroupType.Enemies;
        crea.set<Team>(Team.B);

        var swipe = Register.Create<SpellInstance>();
        swipe.modelUid = Diamonds.spells["spell_swipe"].entityUid;
        crea.activeSkills.add(swipe);

        return crea;
    }

    private static String getRandomCreatureMeshSceneVariant()
    {
        var r = new Random();
        var index = r.Next(creatureMeshSceneVariantNames.Length);
        return creatureMeshSceneVariantNames[index];
    }

}
