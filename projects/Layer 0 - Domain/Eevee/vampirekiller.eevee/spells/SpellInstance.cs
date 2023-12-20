using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using vampirekiller.eevee;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.vampirekiller.eevee.spells;

public class SpellInstance : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    public ID modelUid { get; set; }
    public SmartList<IStatement> statements { get => getModel().statements; set => throw new NotImplementedException(); }

    public StatsDic stats = Register.Create<StatsDic>();

    protected SpellInstance() { }


    public SpellModel getModel() => Diamonds.spells[modelUid];
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}


public class SpellCharges : StatInt { }
public class SpellCooldown : StatInt { }
