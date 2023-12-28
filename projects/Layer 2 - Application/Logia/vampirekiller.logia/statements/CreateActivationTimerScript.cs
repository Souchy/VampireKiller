using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Util.communication.events;
using Util.entity;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.logia.extensions;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using static System.Collections.Specialized.BitVector32;
using Timer = System.Timers.Timer;

namespace vampirekiller.logia.statements;

public class CreateActivationTimerScript : IStatementScript
{
    public Type schemaType => typeof(CreateActivationTimerSchema);

    public void apply(ActionStatementTarget action)
    {
        var actionTrigger = action.getParent<ActionTriggerOnStatusAdd>();
        if(actionTrigger == null) 
            return;
        Status status = actionTrigger.status;

        var props = action.statement.GetProperties<CreateActivationTimerSchema>();

        var activationPeriod = action.getSourceEntity().getTotalStat<SpellTotalCastTime>(props.stats);

        var actimer = new ActivationTimer(status, activationPeriod.value, (sender, args) =>
        {
            GD.Print("ActivationTimer: Activate");
            //var sub = new ActionStatementTarget(action)
            //{

            //};
            props.applyStatementContainer(action); //props.activationStatements
        });
        actimer.start();
    }
}

public class ActivationTimer
{
    private Timer _timer;
    //private Status _status;
    public ActivationTimer(Status status, double activationPeriod, ElapsedEventHandler onElapsed)
    {
        //this._status = status;
        _timer = new Timer(activationPeriod);
        _timer.Elapsed += onElapsed;
        status.GetEntityBus().subscribe(this);
    }
    public void start() => this._timer.Start();

    [Subscribe(Status.EventRemove)]
    public void onStatusRemove(Status s)
    {
        GD.Print("ActivationTimer: Remove");
        s.GetEntityBus().unsubscribe(this);
        //_status?.GetEntityBus()?.unsubscribe(this);
        this._timer?.Stop();
    }
}
