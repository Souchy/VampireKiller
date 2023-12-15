using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using Util.entity;

namespace Espeon.commands
{
    public class CommandPlayGame : ICommand
    {
    }

    public class CommandHandlerPlayGame : BaseCommandHandler<CommandPlayGame>
    {
        private EspeonState state;

        public CommandHandlerPlayGame(EspeonState state) 
        {
            this.state = state;
        }

        public override void handle(CommandPlayGame command)
        {
            this.state.GetEntityBus().publish("fight.started", this.state.fight);
        }
    }
}
