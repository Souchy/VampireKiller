using Glaceon.scenes.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee;

namespace Glaceon;

/// <summary>
/// Auto-loaded by godot
/// </summary>
public class Main
{
    // public Diamonds diamonds = new();
    public static readonly Fight fight = new Fight();
    public static readonly SceneManager sceneManager = SceneManager.instance;

    public void main()
    {
        fight.creatures.GetEntityBus().subscribe(sceneManager);
        fight.projectiles.GetEntityBus().subscribe(sceneManager);
    }

}
