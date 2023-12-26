using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.net;

namespace vampirekiller.umbreon;

/// <summary>
/// Apparently needs to be a node to have the Multiplayer thing
/// </summary>
public partial class UmbreonNet : Node //, Net
{
    public int port = 7000;
    public string address = "host.docker.internal";

    public ENetMultiplayerPeer peer = new();

    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _Ready()
    {
        this.OnReady();
        this.Inject();
        this.Name = this.GetType().Name;
        Umbreon.net = this;

        var error = peer.CreateClient(address, port);
        if (error != Error.Ok)
        {
            GD.Print("Umbreon error cannot client:" + error.ToString());
            return;
        }
        Multiplayer.MultiplayerPeer = peer;
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ServerDisconnected += ServerDisconnected;
        Multiplayer.ConnectionFailed += ConnectionFailed;

        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

        Universe.isOnline = true;
        GD.Print("UmbreonNet ready 3");
    }

    /// <summary>
    /// Receive command packet and pass it to EspeonCommandManager
    /// </summary>
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void onPacketCommand(byte[] bytes)
    {
        GD.Print("Umbreon: onPacketCommand: " + bytes);
        //string json = Encoding.UTF8.GetString(bytes);
        //var command = Json.deserialize<ICommand>(json);
        ////ICommand command = null; // deserialize bytes
        //_commandManager.handle(command);
    }

    /// <summary>
    /// Runs when a player connects and runs on all peers
    /// </summary>
    private void PeerConnected(long id)
    {
        GD.Print("Umbreon Peer connected " + id);
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void PeerDisconnected(long id)
    {
        GD.Print("Umbreon Peer disconnected " + id);
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectedToServer()
    {
        GD.Print("Umbreon Connected to server");
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectionFailed()
    {
        GD.Print("Umbreon Connection failed");
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void ServerDisconnected()
    {
        GD.Print("Umbreon Server disconnected");
    }

}
