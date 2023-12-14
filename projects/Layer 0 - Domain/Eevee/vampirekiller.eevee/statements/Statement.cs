using Godot;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.conditions;

namespace VampireKiller.eevee.vampirekiller.eevee.statements;


public interface IStatement : Identifiable
{
    //public ID modelUid { get; set; }
    public IStatementSchema schema { get; set; }
    public ICondition sourceCondition { get; set; }
    public ICondition targetFilter { get; set; }
    // TDDO create zone classes
    //public IZone TargetAcquisitionZone { get; set; } = new Zone();
    // TODO create Triggers
    //public SmartList<ITriggerModel> Triggers { get; set; } //= new EntityList<ITriggerModel>();
    public SmartList<IStatement> children { get; set; } //= new EntityList<ObjectId>();

    public T GetProperties<T>() where T : IStatementSchema => (T) schema;

    public IStatement copy();
}
public class Statement : IStatement
{
    public ID entityUid { get; set; }
    //public ID modelUid { get; set; }
    public IStatementSchema schema { get; set; }
    public ICondition sourceCondition { get; set; }
    public ICondition targetFilter { get; set; }
    // TDDO create zone classes
    //public IZone TargetAcquisitionZone { get; set; } = new Zone();
    // TODO create Triggers
    //public SmartList<ITriggerModel> Triggers { get; set; } //= new EntityList<ITriggerModel>();
    public SmartList<IStatement> children { get; set; } = SmartList<IStatement>.Create(); //public SmartList<ID> effectIds { get; set; } //= new EntityList<ObjectId>();
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
    public void Dispose()
    {
        this.DisposeEventBus();
    }
}

public interface IStatementSchema
{
    public IStatementSchema copy();
}

public interface IStatementScript
{
    public void apply(IStatement s, Vector3 position, CreatureInstance caster, CreatureInstance currentTarget, IEnumerable<CreatureInstance> allTargetsInZone);
}