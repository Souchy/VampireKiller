using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.communication.commands;

public interface ICommand
{
}

public interface ICommandHandler<T> where T : ICommand
{
    public void handle(T t);
}

// Generics giving me issues, was not able to have multiple handlers in the same list (where be thy wildcard generic mister C#)
public interface ICommandHandler
{
    public void handle(ICommand t);
}
public abstract class BaseCommandHandler<T> : ICommandHandler, ICommandHandler<T> where T : ICommand
{
    public abstract void handle(T command);
    public void handle(ICommand command)
    {
        if (command is T concreteCommand)
        {
            handle(concreteCommand);
        }
    }
}

public interface ICommandPublisher
{
    public void publish(ICommand command);
}