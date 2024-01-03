using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.eevee.util.json;
using Json = vampirekiller.eevee.util.json.Json;

namespace vampirekiller.logia.net;

public partial class RpcClient : Node, IRpcClient
{
    [Inject]
    public ICommandManager _commandManager { get; set; }

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Inject();
        IRpcClient.Instance = this;
        this.Name = this.GetType().Name;
    }

    /// <summary>
    /// Interface method for Logia
    /// Receive command packet and pass it to EspeonCommandManager
    /// </summary>
    public void sendCommand(params Variant[] args) => RpcId(1, nameof(rpcCommand), args);

    /// <summary>
    /// Actual RPC
    /// </summary>
    /// <param name="bytes"></param>
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void rpcCommand(byte[] bytes)
    {
        //GD.Print("Hello rpc input " + this.Multiplayer.GetUniqueId() + ", from " + this.Multiplayer.GetRemoteSenderId()); // + ", args: " + castSlot);
        string json = Encoding.UTF8.GetString(bytes);
        var command = Json.deserialize<ICommand>(json);
        //GD.Print("Json: " + json);
        _commandManager.handle(command);
    }

}
