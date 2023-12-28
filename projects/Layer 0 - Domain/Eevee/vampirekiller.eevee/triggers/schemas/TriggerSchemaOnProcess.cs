
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.triggers.schemas;


public class TriggerSchemaOnProcess : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onProcess;
    // pourrait avoir des propriétés à check


    //public DateTime
    /// <summary>
    /// SpellBaseCastTime = activation time
    /// </summary>
    public StatsDic stats { get; set; }  = Register.Create<StatsDic>();

    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
