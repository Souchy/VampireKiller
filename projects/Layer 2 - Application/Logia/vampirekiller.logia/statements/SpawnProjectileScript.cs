using Eevee.vampirekiller.eevee.stats.schemas;
using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

    private const float bodyHeight = 0.5f;
    private const float rainHeight = 10f;
    private const float angleBetweenProjs = 15;
    private static readonly Random rnd = new Random();

    public void apply(ActionStatementTarget action)
    {
        // ActionCastActive castAction = action.getParent<ActionCastActive>();
        CreatureInstance caster = (CreatureInstance) action.getSourceEntity();
        SpawnProjectileSchema schema = (SpawnProjectileSchema) action.statement.schema;

        // Stats
        bool fireAsRain = caster.getTotalStat<ProjectileFireAsRain>(schema.stats).value;
        bool fireInCircle = caster.getTotalStat<ProjectileFireInCircle>(schema.stats).value;
        int projectileCount = caster.getTotalStat<ProjectileAddCount>(schema.stats).value;
        double speed = caster.getTotalStat<ProjectileTotalSpeed>(schema.stats).value;

        if (fireAsRain)
            fireRain(action, caster, schema, projectileCount, speed);
        else
        if (fireInCircle)
            fireCircle(action, caster, schema, projectileCount, speed);
        else
            fireNormal(action, caster, schema, projectileCount, speed);
    }

    private void fireRain(ActionStatementTarget action, CreatureInstance caster, SpawnProjectileSchema schema, int projectileCount, double speed)
    {
        var rainRadius = caster.getTotalStat<ProjectileRainTotalRadius>(schema.stats);
        Vector3 mouseTarget = action.raycastPosition;
        Vector3 baseDir = (mouseTarget - caster.position).Normalized();
        baseDir.Y = -1;
        Vector3 dir = baseDir.Normalized();

        for (int i = 0; i < projectileCount; i++)
        {
            ProjectileInstance proj = ProjectileInstance.create();
            proj.meshScenePath = schema.scene;
            proj.set<Team>(caster.get<Team>());
            foreach (var s in schema.children)
            {
                proj.statements.add(s);
            }

            double distanceFromCenter = rnd.NextDouble() * rainRadius.value;
            double angle = rnd.NextDouble() * 360;
            double rad = angle * Math.PI / 180.0;
            float opp = (float) (Math.Sin(rad) * distanceFromCenter);
            float adj = (float) (Math.Cos(rad) * distanceFromCenter);
            Vector3 spawnPosition = new Vector3(adj, rainHeight, opp) + mouseTarget; // TODO: mouseTarget should be constrained by the range of the skill


            proj.spawnPosition = spawnPosition;
            proj.init(caster, dir, speed, schema.scene);
            proj.RegisterEventBus();
            ((Identifiable) proj).initialize();
            action.fight.projectiles.add(proj);
        }
    }

    private void fireCircle(ActionStatementTarget action, CreatureInstance caster, SpawnProjectileSchema schema, int projectileCount, double speed)
    {
        float radius = schema.spawnOffset;
        double circlewAngleBetweenProjs = 360.0 / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            ProjectileInstance proj = ProjectileInstance.create();
            proj.meshScenePath = schema.scene;
            proj.set<Team>(caster.get<Team>());
            foreach (var s in schema.children)
            {
                proj.statements.add(s);
            }

            double angle = circlewAngleBetweenProjs * i;
            double rad = angle * Math.PI / 180.0;
            float opp = (float) (Math.Sin(rad) * radius);
            float adj = (float) (Math.Cos(rad) * radius);
            Vector3 dir = new Vector3(adj, 0, opp).Normalized();
            Vector3 spawnPosition = caster.position + dir;
            spawnPosition.Y = bodyHeight;

            proj.spawnPosition = spawnPosition;
            proj.init(caster, dir, speed, schema.scene);
            proj.RegisterEventBus();
            ((Identifiable) proj).initialize();
            action.fight.projectiles.add(proj);
        }
    }
    private void fireNormal(ActionStatementTarget action, CreatureInstance caster, SpawnProjectileSchema schema, int projectileCount, double speed)
    {

        // Spawn math
        float halfAngle = angleBetweenProjs / 2;
        float radius = schema.spawnOffset;

        Vector3 mouseTarget = action.raycastPosition;
        Vector3 basePos = caster.position;
        Vector3 baseDir = (mouseTarget - caster.position).Normalized();
        Vector3 centerPos = baseDir * radius;
        var spawnPosition = basePos + centerPos;
        spawnPosition.Y = bodyHeight;

        //    |      creature in the center
        //   | |     2 proj spread
        //  | | |    3 proj spread
        // | | | |   4 proj spread
        // even: 7.5, -7.5, 22.5, -22.5
        // odd: 0, 15, -15, 30, -30

        int isTotalOdd = (projectileCount % 2);
        int isTotalEven = 1 - isTotalOdd;
        float startAngle = -(isTotalEven * halfAngle);
        var previousAngle = 0.0f;
        var slope = baseDir.Z / baseDir.X;
        var baseAngle = Math.Atan(slope);
        if (baseDir.X < 0)
            baseAngle -= Math.PI;
        //var baseAngleDeg = (baseAngle / Math.PI) * 180.0;

        for (int i = 0; i < projectileCount; i++)
        {
            ProjectileInstance proj = ProjectileInstance.create();
            proj.meshScenePath = schema.scene;
            proj.set<Team>(caster.get<Team>());
            foreach (var s in schema.children)
            {
                proj.statements.add(s);
            }

            var side2 = i % 2;

            // -1, +1, -1, +1, -1, +1
            var sideMultiplier = (side2 * 2) - 1;
            // 0, +1, -1, +1, -1, +1: pour projectiles impairs
            if (i == 0)
                sideMultiplier += isTotalOdd;
            // inc1: -0, +15, -30, +45, -60, +75
            // inc2:  0, +15, -15, +30, -30, +45  
            var increment = angleBetweenProjs * i * sideMultiplier;
            increment += previousAngle;
            previousAngle = increment;

            // ajoute le 7.5 pour les projectiles pairs.
            var addAngle = startAngle + increment;

            var finalRad = addAngle * Math.PI / 180.0;
            finalRad += baseAngle;
            //var finalAngleDeg = finalRad / Math.PI * 180.0;

            float opp = (float) Math.Sin(finalRad) * radius;
            float adj = (float) Math.Cos(finalRad) * radius;
            Vector3 dir = new Vector3(adj, 0, opp).Normalized();

            proj.spawnPosition = spawnPosition;
            proj.init(caster, dir, speed, schema.scene);
            proj.RegisterEventBus();
            ((Identifiable) proj).initialize();
            action.fight.projectiles.add(proj);
        }
    }


}
