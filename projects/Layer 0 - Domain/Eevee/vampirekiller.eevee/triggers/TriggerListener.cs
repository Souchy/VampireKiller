
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using VampireKiller.eevee.vampirekiller.eevee.zones;

namespace vampirekiller.eevee.triggers;

public class TriggerListener
{
    /// <summary>
    /// TriggerData, holds the TriggerType and trigger properties specific to each schema type
    /// </summary>
    public ITriggerSchema schema { get; set; }
    /// <summary>
    /// { When to react to something }
    /// Wether to insert the triggered effects before or after the cause
    public TriggerOrderType triggerOrderType { get; set; } = TriggerOrderType.After;
    /// <summary>
    /// { Where is the thing we can react to }
    /// Only creatures in that zone can activate the trigger (may not need this if we use glyphs)
    /// </summary>
    public IZone triggerZone { get; set; }
    /// <summary>
    /// { Who we can react to }
    /// Conditions what kind of creature can proc this trigger (observation subject), ex: breed is a demon
    /// </summary>
    public ICondition triggererCondition { get; set; }
    /// <summary>
    /// { How the holder is }
    /// Conditions on the holder of the trigger buff, ex: hp higher than 50
    /// </summary>
    public ICondition holderCondition { get; set; }

    public ITriggerScript getScript() => schema.getScript();
    public TriggerListener copy()
    {
        var copy = new TriggerListener(); //Register.Create<TriggerListener>();
        copy.triggerOrderType = triggerOrderType;
        copy.triggerZone = triggerZone.copy();
        copy.triggererCondition = triggererCondition.copy();
        copy.holderCondition = holderCondition.copy();
        copy.schema = schema.copy();
        return copy;
    }
}
