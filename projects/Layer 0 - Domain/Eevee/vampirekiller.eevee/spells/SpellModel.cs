﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.vampirekiller.eevee.spells;

public class SpellModel : Identifiable
{
    public ID entityUid { get; set; }
    public StatsDic stats = Register.Create<StatsDic>();
    public SmartList<IStatement> statements = SmartList<IStatement>.Create();

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// ??
/// </summary>
public class SpellCost : StatInt
{
    
}
public class SpellCostMana : SpellCost { }