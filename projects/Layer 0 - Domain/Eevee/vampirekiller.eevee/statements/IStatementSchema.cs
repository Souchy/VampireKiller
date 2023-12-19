namespace vampirekiller.eevee.statements;

public interface IStatementSchema
{
    // public StatementActionType statementActionType;
    public IStatementSchema copy();
}

// public enum StatementActionType 
// {
    /*
    ground-targeted raycast-> aoe -> creatures          (ex: )
    creature-targeted raycast -> no aoe -> 1 creature   (ex: )
    auto-target -> aoe sample -> creatures              (ex: forbidden rite)
    ground-targeted raycast-> no aoe -> ground          (ex: glyph, )
    ground-targeted raycast-> no aoe -> ground          (ex: projectile skills: bows, fireballs...)
    */
//     ground
// }
