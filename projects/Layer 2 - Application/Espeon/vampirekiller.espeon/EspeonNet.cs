using Godot;
using Godot.Sharp.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.espeon;
using vampierkiller.logia;
using vampirekiller.espeon.commands;
using vampirekiller.logia.net;

namespace vampirekiller.espeon;

public partial class EspeonNet : Node, Net
{
    public int port = 777;
    public readonly ENetMultiplayerPeer peer = new();

    [Inject]
    private readonly ICommandManager _commandManager;

    public override void _Ready()
    {
        this.OnReady();
        this.Inject();
        Espeon.net = this;
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ServerDisconnected += ServerDisconnected;
        Multiplayer.ConnectionFailed += ConnectionFailed;

        var error = peer.CreateServer(port);
        if (error != Error.Ok)
        {
            GD.Print("Espeon error cannot host server:" + error.ToString());
            return;
        }
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
    }

    /// <summary>
    /// Receive command packet and pass it to EspeonCommandManager
    /// </summary>
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void onPacketCommand(byte[] bytes)
    {
        ICommand command = null; // deserialize bytes
        _commandManager.handle(command);
    }


    /// <summary>
    /// Runs when a player connects and runs on all peers
    /// </summary>
    private void PeerConnected(long id)
    {
        GD.Print("Peer connected " + id);
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void PeerDisconnected(long id)
    {
        GD.Print("Peer disconnected " + id);
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectedToServer()
    {
        GD.Print("Connected to server");
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectionFailed()
    {
        GD.Print("Connection failed");
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void ServerDisconnected()
    {
        GD.Print("Server disconnected");
    }
}
