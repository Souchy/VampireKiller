using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Espeon.commands;
using Godot;
using Util.communication.commands;
using Util.entity;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;

namespace Espeon;

public class EspeonState : Identifiable
{
    public ID entityUid { get; set; }
    public Fight fight { get; set; }
    // Having a state that is used by the commands allows them to pass down the reference of the current fight to the spells so they can add their projectiles to it
    
    // List of players here aswell?

    public EspeonState()
    {
        this.fight = EspeonState.createTestFight();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    private static Fight createTestFight()
    {
        var testCreatureModel = Register.Create<CreatureModel>();
        testCreatureModel.meshScenePath = "res://scenes/db/creatures/Orc.tscn";
        testCreatureModel.iconPath = "res://icon.svg";

        var testCreature1 = Register.Create<CreatureInstance>();
        // Removed second creature because they're spawning on top of each other
        //var testCreature2 = Register.Create<CreatureInstance>();
        testCreature1.model = testCreatureModel;
        //testCreature2.model = testCreatureModel;

        var testFight = new Fight();
        testFight.creatures.add(testCreature1);
        //testFight.creatures.add(testCreature2);
        return testFight;
    }
}

public class EspeonCommandHandler : ICommandHandler<ICommand>
{
    public EspeonState state { get; }
    private List<ICommandHandler> handlers = new List<ICommandHandler>();

    public EspeonCommandHandler()
    {
        this.state = Register.Create<EspeonState>();
        this.handlers.Add(new CommandHandlerPlayGame(this.state));
    }

    public void handle(ICommand t)
    {
        this.handlers.ForEach(h => h.handle(t));
    }
}
