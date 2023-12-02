namespace VampireKiller;

public class StatusStats : Stats
{
    public new StatusStatsDictionary flat => (StatusStatsDictionary)base.baseFlat;
    public new StatusStatsDictionary increase => (StatusStatsDictionary)base.increase;
}

public class StatusStatsDictionary : StatsDictionary
{
    public StatusMaxStacks GetStatusMaxStacks() => this.get<StatusMaxStacks>();
    public StatusMaxDuration GetStatusMaxDuration() => this.get<StatusMaxDuration>();
}

public interface StatusStat { }

public record StatusMaxStacks(int value) : StatInt(value), StatusStat { }
public record StatusMaxDuration(double value) : StatDouble(value), StatusStat { }
