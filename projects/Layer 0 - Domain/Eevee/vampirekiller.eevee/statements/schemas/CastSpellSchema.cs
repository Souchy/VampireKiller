
using Util.entity;

namespace vampirekiller.eevee.statements.schemas;

public class CastSpellSchema : IStatementSchema
{
    public ID spellModelId { get; set; }

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
