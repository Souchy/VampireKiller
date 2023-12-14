﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampireKiller.eevee.vampirekiller.eevee.statements.schemas;

public class CreateStatusSchema : IStatementSchema
{
    public int duration { get; set; }   
    public bool unbewitchable { get; set; }
    public int mergeStrategy { get; set; }
    public List<IStatement> children { get; set; } = new();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
