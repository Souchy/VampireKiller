using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.communication.commands;

public interface ICommand
{

}

public interface ICommandHandler<T>
{
    public void handle(T t);
}

public interface ICommandPublisher
{
    public void publish(ICommand command);
}