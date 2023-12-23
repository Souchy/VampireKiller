
using System.Collections.Immutable;
using System.Reflection;
using vampirekiller.eevee;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.logia;

public static class LogiaDiamonds
{
    public static void loadTypes()
    {
        Diamonds.statementScripts
            = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IStatementScript)) && !t.IsAbstract && t.IsClass)
                .Select(t => (IStatementScript)Activator.CreateInstance(t)!)
                .ToImmutableDictionary(s => s.schemaType, s => s);
        Diamonds.triggerScripts
             = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(ITriggerScript)) && !t.IsAbstract && t.IsClass)
                    .Select(t => (ITriggerScript)Activator.CreateInstance(t)!)
                    .ToImmutableDictionary(s => s.schemaType, s => s);
        Diamonds.conditionScripts
             = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IConditionScript)) && !t.IsAbstract && t.IsClass)
                    .Select(t => (IConditionScript)Activator.CreateInstance(t)!)
                    .ToImmutableDictionary(s => s.schemaType, s => s);
    }
}
