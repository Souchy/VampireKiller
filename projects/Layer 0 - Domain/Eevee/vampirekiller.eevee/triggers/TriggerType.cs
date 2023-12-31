﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Namespace;
using vampirekiller.eevee.triggers.schemas;
// using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.eevee.triggers;

/// <summary>
/// To figure out
/// </summary>
public record TriggerType(Type schemaType)
{
    /*
    onTime (momentType -> onFightStarts -> passives)
    onProcessTick (status check duration)
    onStatusAdd
    onStatusExpire
    onDeath
    onMove/onWalk(process?), 
    onTeleport, onDash
    onSpellCast
    onEffectCast, onEffectReceive //onDamageTaken, onDamageDone
    onCollision
    */
    public static readonly TriggerType onProcess = new(typeof(TriggerSchemaOnProcess));
    public static readonly TriggerType onTime = new(typeof(TriggerSchemaOnMoment));
    public static readonly TriggerType onStatement = new(typeof(TriggerSchemaOnStatement));
    public static readonly TriggerType onCollision = new(typeof(TriggerSchemaOnCollision));
    public static readonly TriggerType onCastActive = new(typeof(TriggerSchemaOnCastActive));
    public static readonly TriggerType onDeath = new(typeof(TriggerSchemaOnDeath));

    public static readonly TriggerType onStatusAdd = new(typeof(TriggerSchemaOnStatusAdd));

}

// public class TriggerTypeAddStats : TriggerType { }
// public class TriggerTypeProcessTick : TriggerType { }
// public class TriggerTypeOnCollision : TriggerType { }
// public class TriggerTypeOnSpellCast : TriggerType { }
// public class TriggerEventMove {}

