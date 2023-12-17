using Godot;
using Godot.Sharp.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.net;

namespace vampirekiller.umbreon;

public partial class UmbreonNet : Node, Net
{
    public int port = 777;
    public string address = "host.docker.internal";

    public ENetMultiplayerPeer peer;

    public UmbreonNet()
    {
        this.OnReady();
        this.Inject();
        Umbreon.net = this;
        peer = new();
        var error = peer.CreateClient(address, port);
        if (error != Error.Ok)
        {
            GD.Print("Umbreon error cannot client:" + error.ToString());
            return;
        }
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
    }

    public override void _Ready()
    {
        this.OnReady();
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ServerDisconnected += ServerDisconnected;
        Multiplayer.ConnectionFailed += ConnectionFailed;
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
