using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.logia;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.zones;
using Xunit.Abstractions;

namespace vampirekiller.gems.spells;

internal class PutridCluster : SpellGenerator
{
    public PutridCluster(ITestOutputHelper output) : base(output)
    {
    }

    protected override SpellModel GenerateSpell()
    {
        var spell = Register.Create<SpellModel>();
        spell.entityUid = "spell_putrid_cluster"; // Set un ID constant pour pouvoir load toujours le même
        spell.iconPath = Paths.spells + "shock_nova/yellow_17.PNG";
        spell.skins.Add(new()
        {
            animationLibraries = new() { "pro_magic_pack" },
            sourceAnimation = "pro_magic_pack/Standing 2H Magic Area Attack 01"
        });
        spell.stats.set(new SpellBaseCastTime() { value = 0.6 });
        spell.stats.set(new SpellBaseCostMana() { value = 1 });

        var spawnZone = new Statement();
        spawnZone.zone.size = new ZoneSize(3);
        spawnZone.zone.samplingType = ZoneSamplingType.random;
        spawnZone.zone.minSampleCount = 5;
        spawnZone.zone.maxSampleCount = 5;
        spawnZone.schema = new SpawnFxSchema()
        {
            follow = false,
            scene = ""
        };

        var timer = new CreateActivationTimerSchema();
        timer.stats.set(new SpellBaseCastTime() { value = 1 });
        var timerStatement = new Statement()
        {
            zone = new Zone() { worldOrigin = ZoneOriginType.Target },
            schema = timer
        };
        spawnZone.statements.add(timerStatement);

        spell.statements.add(spawnZone);
        return spell;
    }
}
