using SimpleInjector;
using Util.communication.commands;
using vampierkiller.logia;

namespace vampirekiller.espeon;

public static class EspeonServices
{
    public static void RegisterEspeon(this Container container)
    {
        var commands = container.GetInstance<ICommandManager>();
        // TODO register Espeon command handlers
        //commands.setHandler(new HandlerOnPlay());
        //commands.setHandler(new HandlerOnExitToMain());
    }
}