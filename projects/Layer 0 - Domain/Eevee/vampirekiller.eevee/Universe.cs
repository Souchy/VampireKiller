using Godot;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.communication.events;
using VampireKiller.eevee.vampirekiller.eevee;
using Container = SimpleInjector.Container;

namespace Logia.vampirekiller.logia;

public class Universe
{
    public static bool isServer { get; set; }
    public static bool isOnline { get; set; }

    public static Container container { get; set; }

    //public static Node root;

    private static Fight _fight;
    public static Fight fight
    {
        get => _fight;
        set
        {
            _fight = value;
            EventBus.centralBus.publish(Fight.EventSet, fight);
        }
    }

    /// <summary>
    /// Sapphire
    /// </summary>
    public static Node root { get; set; }
    public static Godot.Timer createTimer(System.Action lambda, double activationPeriod)
    {
        var timer = new Godot.Timer();
        timer.AddToGroup("timers");
        timer.WaitTime = activationPeriod;
        timer.Timeout += lambda;
        root.AddChild(timer);
        return timer;
    }

}
