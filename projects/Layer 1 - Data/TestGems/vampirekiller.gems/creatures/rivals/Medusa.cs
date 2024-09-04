using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;
using vampirekiller.eevee.stats.schemas.resources;
using vampirekiller.logia;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;
using Xunit.Abstractions;

namespace vampirekiller.gems.creatures.rivals;

public class Medusa : CreatureGenerator
{
    public Medusa(ITestOutputHelper output) : base(output)
    {
    }

    protected override CreatureModel GenerateCreature()
    {
        var creaModel = Register.Create<EnemyModel>();
        // Stats
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 100;
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 100;
        creaModel.baseStats.set(new CreatureBaseMovementSpeed() { value = 3 });
        // Ai, skills
        creaModel.ai = new AiMelee();
        creaModel.baseSkillIds.Add("spell_swipe");
        // Skin
        var skin = new CreatureSkin()
        {
            iconPath = "res://icon.svg",
            scenePath = Paths.creatures + "rivals/small/character_medusa_01",
            //animationLibraries = StubFight.animLibraries,
            animationLibraries = new() { "baked/combined.baked" },
            //animations = StubFight.anims
            animations = new()
            {
                idle = "1-7",
                idleOneShots = Array.Empty<string>(),
                run = "1-10",
                walk = "1-12",
                cast = "1-1",
                receiveHit = "1-9",
                death = "1-6",
                victory = "",
                defeat = "1-5"
            }
        };
        creaModel.skins.Add(skin);
        return creaModel;
    }
}
