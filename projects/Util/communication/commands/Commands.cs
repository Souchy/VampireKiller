using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.communication.commands;

public interface ICommand
{
    public bool preferOnline => true;
    public byte[] serialize();
}

public interface ICommandHandler<T> where T : ICommand
{
    public void handle(T command);
}

public interface ICommandPublisher
{
    public void publish<T>(T command) where T : ICommand;
    public Task publishAsync<T>(T command) where T : ICommand;
}

public interface ICommandManager
{
    public void handle<T>(T command) where T : ICommand;
    public Task handleAsync<T>(T command) where T : ICommand;
    public void setHandler<T>(ICommandHandler<T> handler) where T : ICommand;
    public void setHandler<T>(Action<T> handler) where T : ICommand;
}
public class CommandManager : ICommandManager
{
    private Dictionary<Type, Action<ICommand>> commandHandlers = new();

    public void handle<T>(T command) where T : ICommand
    {
        if (commandHandlers.ContainsKey(command.GetType()))
        {
            var handler = commandHandlers[command.GetType()];
            handler(command);
        }
    }

    public async Task handleAsync<T>(T command) where T : ICommand
    {
        await Task.Run(() => handle(command));
    }

    public void setHandler<T>(ICommandHandler<T> handler) where T : ICommand
    {
        commandHandlers[typeof(T)] = (ICommand i) => handler.handle((T) i);
    }

    public void setHandler<T>(Action<T> handler) where T : ICommand
    {
        commandHandlers[typeof(T)] = (ICommand i) => handler((T) i);
    }
}