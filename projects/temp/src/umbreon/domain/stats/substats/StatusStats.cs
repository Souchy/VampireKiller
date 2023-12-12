namespace VampireKiller;

public interface StatusStat { }


/// <summary>
/// Model stats
/// </summary>

public class StatusStats : StatsDictionary
{
    public StatusMaxStacks GetStatusMaxStacks() => this.get<StatusMaxStacks>();
    public StatusMaxDuration GetStatusMaxDuration() => this.get<StatusMaxDuration>();
}

public record StatusMaxStacks(int value) : StatInt(value), StatusStat { }
public record StatusMaxDuration(double value) : StatDouble(value), StatusStat { }



/// <summary>
/// Instance stats
/// </summary>
public class StatusInstanceStats : StatsDictionary
{
    // public StatusStacks stacks { get; set; }
    // public StatusDuration duration { get; set; }
    public StatusStacks GetStatusStacks() => this.get<StatusStacks>();
    public StatusDuration GetStatusDuration() => this.get<StatusDuration>();
}
public record StatusStacks(int value) : StatInt(value), StatusStat { }
public record StatusDuration(double value) : StatDouble(value), StatusStat { }
