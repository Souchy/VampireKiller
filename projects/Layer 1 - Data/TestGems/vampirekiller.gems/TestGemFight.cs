using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eevee.vampirekiller.eevee.stats.schemas;
using Util.entity;
using VampireKiller.eevee.creature;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.stats.schemas.resources;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using Godot;
using VampireKiller.eevee.vampirekiller.eevee;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.vampirekiller.eevee.equipment;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.gems;

public class TestGemFight : Fight
{
    public CreatureInstance getFirstCreature(bool isAlly)
    {
        return this.creatures.values.First(c =>
            isAlly
                ? c.creatureGroup == EntityGroupType.Players
                : c.creatureGroup == EntityGroupType.Enemies
        );
    }

    public static CreatureInstance spawnStubCreature(bool isAlly)
    {
        return spawnStubCreature(isAlly, new Vector3(0, 0, 0));
    }

    public static CreatureInstance spawnStubCreature(bool isAlly, Vector3 position)
    {
        var creaModel = Register.Create<CreatureModel>();
        creaModel.baseStats.get<CreatureBaseLifeMax>()!.value = 100;
        creaModel.baseStats.get<CreatureBaseLife>()!.value = 100;

        var crea = Register.Create<CreatureInstance>();
        crea.model = creaModel;
        crea.spawnPosition = position;

        if (isAlly) {
            crea.creatureGroup = EntityGroupType.Players;
            crea.set<Team>(Team.A);
        } else {
            crea.creatureGroup = EntityGroupType.Enemies;
            crea.set<Team>(Team.B);
        }

        return crea;
    }
}
