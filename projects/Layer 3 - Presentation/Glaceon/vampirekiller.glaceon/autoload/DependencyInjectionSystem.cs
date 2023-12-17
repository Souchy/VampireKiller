using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Container = SimpleInjector.Container;
using vampierkiller.logia;
using vampirekiller.espeon;
using Logia.vampirekiller.logia;
using vampirekiller.umbreon;
using Util.communication.commands;
using SimpleInjector;

namespace vampirekiller.glaceon.autoload;

public partial class DependencyInjectionSystem : Node, IDependencyInjectionSystem
{
    private Container container;
    public override void _EnterTree()
    {
        base._EnterTree();
        Universe.container = container = new Container();
        registerServices();
        registerApps();
        // GD.Print("DI registered");
    }

    public object Resolve(Type type) => container.GetInstance(type);

    private void registerServices()
    {
        container.Register<ICommandManager, CommandManager>(Lifestyle.Singleton);
    }

    private void registerApps()
    {
        // Always need the Espeon container if we want to host a server inside the same app process as the client.
        if (Universe.isServer) 
            container.RegisterEspeon();
        // But if we're in server/headless mode, we don't need Umbreon
        if (!Universe.isServer)
        {
            container.RegisterUmbreon();
        }
    }

}
