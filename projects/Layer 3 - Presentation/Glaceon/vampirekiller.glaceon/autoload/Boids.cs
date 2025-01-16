using Godot;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using vampirekiller.eevee.creature;
using VampireKiller.eevee.creature;

namespace vampirekiller.glaceon.autoload;


public class BoidClan
{
    // 2d position
    public Vector2[] positions = new Vector2[CrowdInstance.maxCreatureCount]; // Vector<T> cant update singular values (ex: direction)
    // 2d orientation
    public Vector2[] directions = new Vector2[CrowdInstance.maxCreatureCount];
    // x(1) = x(0) + dT * speed * direction
    public double[] speeds = new double[CrowdInstance.maxCreatureCount];

    //public int[] clans;
    public float[] angles;

    //public Vector2[] positions;
    //public Vector2[] directions;
    //public double[] speeds;
    //public float[] angles;
}

public class Boids
{

    public Boids()
    {

    }

    public static List<BoidClan> crowds = new();

    private void updateThread()
    {
        long a = 0;
        long b = 0;
        while (true)
        {
            a = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Thread.Sleep(1000 / 30);
            b = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var delta = b - a;
            //updateCrowds(b - a);
            foreach (BoidClan c in crowds)
            {
                for (int i = 0; i < c.positions.Length; i++)
                {
                    updateBoid(c, i, delta);
                }
            }
        }
    }
    private CreatureInstance player;
    //private void updateCrowds(long deltaTime)
    //{
    //    if (player == null)
    //    {
    //        player = Universe.fight.creatures.get(c => c.playerId != 0);
    //    }

    //    foreach(var crowd in crowds)
    //    //foreach (var crowd in Universe.fight.crowds.values)
    //    {
    //        int length = crowd.positions.Length;
    //        for (int i = 0; i < length; i++)
    //        {

    //            crowd.positions[i] += crowd.angles[i] * crowd.speeds[i]* deltaTime;
    //        }
    //    }
    //}

    public float Separation { get; set; } = 1f;
    public float Cohesion { get; set; } = 1f;
    public float Alignment { get; set; } = 1f;
    public float DetectRadius { get; set; } = 4f;
    public float PersonalRadius { get; set; } = 1f;
    public float BaitWeight { get; set; } = 3;
    public float WanderWeight { get; set; } = 1f;

    public void updateBoid(BoidClan crowd, int i, double delta)
    {
        // var pos0 = crowd.positions[i];
        // var vel0 = crowd.directions[i];

        // Vector2 avgPos = Vector2.Zero;
        // Vector2 avgVel = Vector2.Zero;
        // Vector2 close_d = Vector2.Zero;

        // // Wandering
        // Vector2 wander = vel0 * (float) Math.Tan(2f * Math.PI / 12f);
        // wander *= (float) Math.Sin(DateTime.Now.Ticks / TimeSpan.TicksPerSecond);

        // // Bait
        // var baitInRadius = false;
        // Vector2 baitAttraction = Vector2.Zero;
        // for (int j = 0; j < 1; i++) //BaitShapes.Count; j++)
        // {
        //     //var bait = BaitShapes[j];
        //     //var bait2d = baitPositions[j];
        //     var bait2d = new Vector2(player.position.X, player.position.Y);

        //     var dist = bait2d - pos0;
        //     var d = dist.Length();

        //     if (d <= PersonalRadius)
        //     {
        //         close_d += -dist; // * (float) Math.Tan(2f * Math.PI / 8f) * 2f;

        //         baitInRadius = true;
        //         float angle = (float) (Math.Atan2(dist.Y, dist.X) % (2 * Math.PI) - Math.PI / 2.0);
        //         crowd.angles[i] += (angle - crowd.angles[i]); // * (float) delta * 3f;
        //     }
        //     else
        //     if (d <= DetectRadius * 2)
        //     {
        //         baitInRadius = true;
        //         float angle = (float) (Math.Atan2(dist.Y, dist.X) % (2 * Math.PI) - Math.PI / 2.0);
        //         crowd.angles[i] += (angle - crowd.angles[i]);
        //         var offvel = dist - vel0;
        //         //baitAttraction += offvel;
        //         //baitAttraction += dist;
        //         //baitAttraction += new Vector2(dist.Y, dist.X);
        //         //baitAttraction += offvel * (float) Math.Cos(2f * Math.PI / 4f);
        //         //baitAttraction += offvel * (float) Math.Tan(2f * Math.PI / 8f);
        //         baitAttraction += dist.Normalized() * (float) Math.Tan(2f * Math.PI / 4f);
        //         //baitAttraction += dist * (float) Math.Tan(2f * Math.PI / 5f);
        //         //baitAttraction += Vector2.Up;

        //     }
        // }
        // float baitMulti = baitInRadius ? 0.0f : 1.0f;

        // // Obstacles
        // //for (int j = 0; j < ObstacleShapes.Count; j++)
        // //{
        // //    var obs = ObstacleShapes[j];
        // //    var pos1 = obstaclePositions[j];

        // //    var dist = pos1 - pos0;
        // //    var d = dist.Length();
        // //    if (d <= PersonalRadius)
        // //    {
        // //        close_d -= dist; // * (float) Math.Tan(2f * Math.PI / 8f);
        // //    }
        // //}

        // // Flocking
        // int countInProximity = 0;
        // for (int j = 0; j < Count; j++)
        // {
        //     if (i == j) continue;
        //     var pos1 = crowd.positions[j];
        //     var vel1 = crowd.directions[j];
        //     var dist = pos0 - pos1;
        //     var d = dist.Length();

        //     if (d <= PersonalRadius)
        //     {
        //         close_d += dist * (float) Math.Tan(2f * Math.PI / 8f);
        //     }
        //     else
        //     if (d <= DetectRadius && crowd.clans[i] == crowd.clans[j] && vel0.Normalized().Dot(-dist) > -0.5f)
        //     {
        //         countInProximity++;
        //         avgPos += pos1;
        //         avgVel += vel1;
        //     }
        // }

        // // Apply
        // if (countInProximity > 0)
        // {
        //     avgPos /= countInProximity;
        //     avgVel /= countInProximity;

        //     var cohesion = (avgPos - pos0) * Cohesion * baitMulti;
        //     var align = (avgVel - vel0) * Alignment * baitMulti;
        //     vel0 += cohesion + align;
        // }


        // vel0 += baitAttraction * BaitWeight;
        // vel0 += wander * WanderWeight;
        // vel0 += close_d * Separation;

        // // var speed = Math.Clamp(vel0.Length(), 1, Speed);
        // // vel0 = vel0.Normalized() * speed;

        // //velocities[i] += (vel0 - velocities[i]) * 0.5f; // * (float) delta * 3f;
        // //velocities[i] = vel0;
        // // vel0 = (vel0 + velocities[i]) / 2.0f;
        // // velocities[i] = vel0;
        // // vel0 = velocities[i];

        // // if (!baitInRadius)
        // // {
        // //     float angle = (float) (Math.Atan2(vel0.Y, vel0.X) % (2 * Math.PI) - Math.PI / 2.0);
        // //     crowd.angles[i] = angle;
        // // }

        // // pos0 += vel0 * (float) delta;
        // // if (pos0.X > Bounds.X) pos0.X = 0;
        // // if (pos0.X < 0) pos0.X = Bounds.X;
        // // if (pos0.Y > Bounds.Y) pos0.Y = 0;
        // // if (pos0.Y < 0) pos0.Y = Bounds.Y;
        // // crowd.positions[i] = pos0;
    }



}
