
using Util.structures;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace VampireKiller.eevee.vampirekiller.eevee.spells;

public interface IStatementContainer
{
    public SmartList<IStatement> statements { get; set; }
}
