using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Util.ecs;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;

namespace vampirekiller.eevee.creature;

public class CrowdInstance : Entity, Identifiable
{
    public SmartList<CreatureInstance> Instances { get; set; } = SmartList<CreatureInstance>.Create();
    private CrowdInstance()
    {
        //Instances.add()
    }

    // TODO 
    // data-oriented avec size prédéterminée évite les problèmes de multithread concurrency, plus rapide itération, évite les cache miss, permet SIMD
    // Les lists ont toujours 500 slots, mais le actual creature count est atomic
    public const int maxCreatureCount = 100; // 500
    public volatile int actualCreatureCount = 0;

    //// 2d position
    //public Vector2[] positions = new Vector2[maxCreatureCount]; // Vector<T> cant update singular values (ex: direction)
    //// 2d orientation
    //public Vector2[] directions = new Vector2[maxCreatureCount];
    //// x(1) = x(0) + dT * speed * direction
    //public double[] speeds = new double[maxCreatureCount];

    //public int[] clans;
    //public float[] angles;

    public int poolCreatureInstance()
    {
        for (int i = 0; i < maxCreatureCount; i++)
        {
            if (Instances.getAt(i)!.isActive)
                return i;
        }
        return -1;
    }

}
