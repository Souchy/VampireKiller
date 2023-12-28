using Godot;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using vampirekiller.eevee;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace VampireKiller.eevee.vampirekiller.eevee.statements;


public interface IStatement : IStatementContainer //: Identifiable
{
    //public ID modelUid { get; set; }
    public IStatementSchema schema { get; set; }
    public ICondition sourceCondition { get; set; }
    public ICondition targetFilter { get; set; }
    // TDDO create zone classes
    public IZone zone { get; set; }
    // TODO create Triggers
    public SmartList<TriggerListener> triggers { get; set; }
    public bool isRequiredForContainer { get; set; }

    public T GetProperties<T>() where T : IStatementSchema => (T) schema;
    public IStatement copy();
}
public class Statement : IStatement
{
    // public ID entityUid { get; set; }
    //public ID modelUid { get; set; }
    /// <summary>
    public IStatementSchema schema { get; set; }
    public ICondition sourceCondition { get; set; }
    public ICondition targetFilter { get; set; }
    public IZone zone { get; set; } = new Zone();
    public SmartList<TriggerListener> triggers { get; set; } = SmartList<TriggerListener>.Create();
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();
    /// If true, when cancelled, cancels the whole statement container (spell, etc). <br></br>
    /// Otherwise, just dont apply this statement, but still apply the others
    /// </summary>
    public bool isRequiredForContainer { get; set; }

    public IStatement copy()
    {
        var copy = new Statement();
        //copy.modelUid = modelUid;
        copy.schema = schema.copy();
        copy.sourceCondition = sourceCondition?.copy();
        copy.targetFilter = targetFilter?.copy();
        //copy.targetAcquisitionZone = targetAcquisitionZone.copy();
        //foreach (var trigger in triggers.Values)
        //    copy.triggers.Add(trigger.copy());
        //foreach (var effect in this.GetEffects().Where(effect => effect != null))
        //    copy.EffectIds.Add(EffectInstance.Create(copy.fightUid, effect).entityUid);
        return copy;
    }
    // public void Dispose()
    // {
    //     this.DisposeEventBus();
    // }
}


public interface IStatementScript
{
    public Type schemaType { get; }
    // TODO replace with ActionEffectTarget
    public void apply(ActionStatementTarget action);
    // public void apply(IStatement s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget, IEnumerable<CreatureInstance> allTargetsInZone);
}
