using Eevee.vampirekiller.eevee.stats.schemas;
using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.logia.extensions;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.logia.statements;

/// <summary>
///
/// </summary>
public class SpawnProjectileScript : IStatementScript
{
    public Type schemaType => typeof(SpawnProjectileSchema);

    public void apply(ActionStatementTarget action)
    {
        // ActionCastActive castAction = action.getParent<ActionCastActive>();
        CreatureInstance caster = (CreatureInstance) action.getSourceEntity();
        Vector3 mouseTarget = action.raycastPosition;
        SpawnProjectileSchema schema = (SpawnProjectileSchema)action.statement.schema;

        // Stats
        bool fireInCircle = caster.getTotalStat<ProjectileFireInCircle>(schema.stats).value;
        int projectileCount = caster.getTotalStat<ProjectileAddCount>(schema.stats).value;

        // Spawn math
        // Vector3 basePoint = caster.position + schema.spawnOffset;
        float offsetLength = schema.spawnOffset.Length();
        float angleBetweenProjs = 15;
        float halfAngle = angleBetweenProjs / 2;
        int speed = 2;

        
        //    |      creature in the center
        //   | |     2 proj spread
        //  | | |    3 proj spread
        // | | | |   4 proj spread
        // even: 7.5, -7.5, 22.5, -22.5
        // odd: 0, 15, -15, 30, -30

        int isTotalEven = 1 - (projectileCount % 2);
        float startAngle = isTotalEven * halfAngle;

        for(int i = 0; i < projectileCount; i++) {
            ProjectileInstance proj = ProjectileInstance.create();

            var side2 = i % 2;

            var sideMultiplier = (side2 * 2) - 1;    // -1, +1, -1, +1, -1, +1
            var increment = angleBetweenProjs * i * sideMultiplier; // -0, +15, -30, +45, -60, +75
            var angle = startAngle + increment;

            float opp = (float) Math.Sin(angle) * offsetLength;
            float adj = (float) Math.Cos(angle) * offsetLength;
            Vector3 spawnPoint = new Vector3(opp, 0, adj);

            
            proj.set<Team>(caster.get<Team>());
            foreach(var s in schema.children) {
                proj.statements.add(s);
            }
            proj.spawnPosition = spawnPoint;
            proj.init(caster, mouseTarget, speed, schema.scene);
            proj.RegisterEventBus();
            ((Identifiable) proj).initialize();
            action.fight.projectiles.add(proj);
        }

    }

}
