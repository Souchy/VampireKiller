using System.Collections.Generic;

namespace VampireKiller;

public class Status
{
    public StatusStats stats = new();
    public List<Statement> effects { get; set; }
}


public class StatusInstance
{
    public StatusInstanceStats stats = new();
    // public StatusInstanceStats stats = new();
}
