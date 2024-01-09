using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using Util.structures;
using vampirekiller.eevee;
using vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee.vampirekiller.eevee.spells;

public class SpellInstance : Identifiable, IStatementContainer
{
    public ID entityUid { get; set; }
    public ID modelUid { get; set; }
    /// <summary>
    /// Set at creation. Mostly depends on the creature skin, could change it later through events too or with MTX.
    /// </summary>
    public SkillSkin skin { get; set; } 

    public SmartList<IStatement> statements { get => getModel().statements; set => throw new NotImplementedException(); }

    public StatsDic stats = Register.Create<StatsDic>();

    protected SpellInstance() { }


    public SpellModel getModel() => Diamonds.spellModels[modelUid];
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}


public class SpellCharges : StatInt { }
public class SpellCooldown : StatInt { }
