using Logia.vampirekiller.logia;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.espeon.commands;
using vampirekiller.logia.stub;
using vampirekiller.umbreon.commands;

namespace vampirekiller.umbreon;

public static class UmbreonServices
{
    public static void RegisterUmbreon(this Container container)
    {
        container.Register<ICommandPublisher, UmbreonCommandPublisher>(Lifestyle.Singleton);

        var commands = container.GetInstance<ICommandManager>();
        commands.setHandler(new HandlerOnPlay());
        commands.setHandler(new HandlerOnExitToMain());
        commands.setHandler(new HandlerOnCast());
        commands.setHandler(new HandlerOnProjectileCollision());
        //commands.setHandler<CommandProcessTick>(command =>
        //{
        //    Universe.fight.creatures.add(StubFight.spawnStubCreature(new Godot.Vector3(15, 1, 15)));
        //});
    }
}
