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
using vampirekiller.eevee.stats.schemas.resources;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;

namespace vampirekiller.logia.stub;

public class StubFight : Fight
{
    private static List<String> animationLibraries = new() { "action_adventure", "pro_magic_pack" };
    private static CreatureAnimationData anims = new()
    {
        idle = "action_adventure/idle (2)", // TODO add oneShot 'idle' + 'idle (3)'
        run = "action_adventure/running", // TODO add transition' run_to_stop' -> idle
        walk = "action_adventure/walking",
        cast = "pro_magic_pack/",
        receiveHit = "pro_magic_pack/Standing React Small From Front",
        death = "pro_magic_pack/Standing React Death Forward",
        victory = "",
        defeat = ""
    };

    public StubFight()
    {
        var player = spawnStubPlayer();
        player.playerId = 1;
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
        //creaModel.meshScenePath = "res://scenes/db/creatures/PlayerCharacters.tscn";
        //creaModel.iconPath = "res://icon.svg";
        var skin = new CreatureSkin()
        {
            iconPath = "res://icon.svg",
            scenePath = "res://VampireAssets\\test\\dg chars\\small\\character_spirit_demon",
        };
        creaModel.skins.Add(skin);
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 100;
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 100;
        creaModel.baseStats.set(new ProjectileAddCount() { value = 2 });
        creaModel.baseStats.set(new ProjectileIncreasedSpeed() { value = 300 });
        creaModel.baseStats.set(new IncreasedDamage() { value = 100 });
        creaModel.baseStats.set(new SpellIncreasedCastSpeed() { value = 100 });

        var crea = Register.Create<CreatureInstance>();
        crea.model = creaModel;
        crea.currentSkin = creaModel.skins[0];
        crea.spawnPosition = new Vector3(0.1f, 0, 0); // pas sur pourquoi ça bug si on déplace pas le player au spawn
        crea.creatureGroup = EntityGroupType.Players;
        crea.set<Team>(Team.A);

        var fireball = Register.Create<SpellInstance>();
        fireball.modelUid = Diamonds.spellModels["spell_fireball"].entityUid;
        crea.activeSkills.add(fireball);

        var shocknova = Register.Create<SpellInstance>();
        shocknova.modelUid = Diamonds.spellModels["spell_shock_nova"].entityUid;
        crea.activeSkills.add(shocknova);


        return crea;
    }

    public static CreatureInstance spawnStubCreature(Vector3 vec)
    {
        var creaModel = Register.Create<EnemyModel>();
        //creaModel.meshScenePath = "res://scenes/db/creatures/EnemyCharacters.tscn";
        //creaModel.iconPath = "res://icon.svg";
        var skin = new CreatureSkin()
        {
            iconPath = "res://icon.svg",
            scenePath = "res://VampireAssets\\test\\dg chars\\small\\character_medusa_01",
            animationLibraries = StubFight.animationLibraries,
            animations = StubFight.anims
        };
        creaModel.ai = new AiMelee();
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 100;
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 100;

        var crea = Register.Create<CreatureInstance>();
        crea.model = creaModel;
        crea.currentSkin = creaModel.skins[0];
        crea.spawnPosition = vec;
        crea.creatureGroup = EntityGroupType.Enemies;
        crea.set<Team>(Team.B);

        var swipe = Register.Create<SpellInstance>();
        swipe.modelUid = Diamonds.spellModels["spell_swipe"].entityUid;
        crea.activeSkills.add(swipe);

        return crea;
    }

}
