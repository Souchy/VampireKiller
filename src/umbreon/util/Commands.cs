
using Godot;

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