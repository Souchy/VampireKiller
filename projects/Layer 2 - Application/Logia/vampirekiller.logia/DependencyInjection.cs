using Godot;
using SimpleInjector;
using souchy.celebi.eevee;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Container = SimpleInjector.Container;

namespace vampierkiller.logia;

#region Low level
public class InjectAttribute : Attribute { }

public interface IDependencyInjectionSystem
{
    public object Resolve(Type type);
}

public static class NodeExtensions
{
    private const string disPath = "/root/DependencyInjectionSystem";
    private static IDependencyInjectionSystem dis;
    private static readonly Type at = typeof(InjectAttribute);

    public static void Inject(this Node node)
    {
        dis ??= node.GetNode<IDependencyInjectionSystem>(disPath);

        var fields = node.GetType()
            .GetMembers()
            .Where(f => f.GetCustomAttributes(at, true).Any());
        // GD.Print("DI Inject in " + node.Name + ", fields: " + string.Join(", ", fields.Select(f => f.Name)));
        foreach (var field in fields)
        {
            var obj = dis.Resolve(field.MemberType());
            // GD.Print("DI resolved: " + field.Name + " = " + obj + ", in " + node.Name);
            try
            {
                field.SetValue(node, obj);
            }
            catch (InvalidCastException)
            {
                GD.PrintErr($"Error converting value " +
                    $"{obj} ({obj.GetType()})" +
                    $" to {field.MemberType()}");
                throw;
            }
        }
    }

    private static Type MemberType(this MemberInfo member)
    {
        if (member is PropertyInfo prop)
            return prop.PropertyType;
        else
        if (member is FieldInfo field)
            return field.FieldType;
        else
            throw new Exception("[Inject] attribute on a wrong member type");
    }

    private static void SetValue(this MemberInfo member, object? obj, object? value)
    {
        if (member is PropertyInfo prop)
            prop.SetValue(obj, value);
        else
        if (member is FieldInfo field)
            field.SetValue(obj, value);
        else
            throw new Exception("[Inject] attribute on a wrong member type");
    }

}
#endregion


