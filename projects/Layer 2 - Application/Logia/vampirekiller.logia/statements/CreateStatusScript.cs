﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VampireKiller.eevee.vampirekiller.eevee.statements.schemas;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.statements.schemas;
using Util.entity;
using VampireKiller.eevee.creature;
using vampirekiller.logia.extensions;
using vampirekiller.eevee.stats.schemas.skill;
using vampirekiller.eevee.conditions.schemas;
using vampirekiller.eevee.triggers.schemas;
using vampirekiller.eevee.triggers;
using VampireKiller.eevee.vampirekiller.eevee.conditions;
using VampireKiller.eevee.vampirekiller.eevee.enums;
using System.Timers;
using Timer = System.Timers.Timer;

namespace vampirekiller.logia.statements;

/// <summary>
/// Could come from a status proc or a skill cast
/// </summary>
public class CreateStatusScript : IStatementScript
{
    public Type schemaType => typeof(CreateStatusSchema);

    public void apply(ActionStatementTarget action)
    {
        // TODO status creation script
        var target = action.currentTarget as CreatureInstance;
        var source = action.getSourceEntity();
        if (target == null || source == null)
            return;

        var castAction = action.getParent<ActionCastActive>();
        action.getContext<CreatureInstance>(IActionTrigger.creature);

        // Gather properties
        var props = action.statement.GetProperties<CreateStatusSchema>();
        var maxDuration = source.getTotalStat<SkillMaxDuration>(props.stats);
        var maxStacks = source.getTotalStat<StatusMaxStacks>(props.stats);
        var stacks = source.getTotalStat<StatusStacks>(props.stats);
        var totalDuration = source.getTotalStat<SkillTotalDuration>(props.stats).value;
        totalDuration = Math.Clamp(totalDuration, 0, maxDuration.value);

        // TODO manage Merge Strategy & max stacks
        // Need to identify status stacks to group them to calculate MaxStacks. -> Use skillSourceId & statusModelID 
        int currentStacks = 0; // creature.statuses.count(s => s....)
        int stacksToAdd = Math.Clamp(stacks.value + currentStacks, 0, maxStacks.value);
        // TODO: on refresh: reset the expirationTimer

        // TODO How do we do stacks? New instance for each?
        // Also, we need icons, names, 
        for (int i = 0; i < stacksToAdd; i++)
        {
            // Create status
            var status = Register.Create<Status>();
            foreach (var statement in props.statusStatements)
            {
                status.statements.add(statement);
            }
            status.stats.set(new SkillExpirationDate() { value = DateTime.Now.AddSeconds(totalDuration) });
            status.stats.set(maxDuration);
            status.stats.set(maxStacks);

            // maybe use a Timer instead of "polling" the processTick lol.
            //status.statements.add(getExpirationStatement());
            var expirationTimer = new Timer(totalDuration);
            expirationTimer.Start();
            expirationTimer.Elapsed += (object? sender, ElapsedEventArgs e) =>
            {
                expirationTimer.Stop();
                target.statuses.remove(status);
            };
            
            var activationTimer = new Timer(1); // StatusTotalActiovationFrequency
            activationTimer.Start();
            activationTimer.Elapsed += (object? sender, ElapsedEventArgs e) =>
            {
                // proc/activate a statement ....
            };

            target.statuses.add(status);
        }

    }

}
