using Godot;
using SimpleInjector;
using souchy.celebi.eevee;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Container = SimpleInjector.Container;
//using CreatureData = souchy.celebi.eevee.impl.objects.Creature;

namespace vampierkiller.logia;

#region Low level
public class InjectAttribute : Attribute { }

public interface IDependencyInjectionSystem
{
    public object Resolve(Type type);
}

public static class NodeExtensions
{
    public static void Inject(this Node node)
    {
        var disPath = "/root/DependencyInjectionSystem";
        var dis = node.GetNode<IDependencyInjectionSystem>(disPath);
        var at = typeof(InjectAttribute);
        var fields = node.GetType()
            .GetProperties()
            //.GetRuntimeFields()
            .Where(f => f.GetCustomAttributes(at, true).Any());
        // GD.Print("DI Inject in " + node.Name + ", fields: " + string.Join(", ", fields.Select(f => f.Name)));
        foreach (var field in fields)
        {
            var obj = dis.Resolve(field.PropertyType); //.FieldType);
            // GD.Print("DI resolved: " + field.Name + " = " + obj + ", in " + node.Name);
            try
            {
                field.SetValue(node, obj);
            }
            catch (InvalidCastException)
            {
                GD.PrintErr($"Error converting value " +
                    $"{obj} ({obj.GetType()})" +
                    $" to {field.PropertyType}");
                throw;
            }
        }
    }
}
#endregion


