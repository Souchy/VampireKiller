using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.commands;
using vampierkiller.espeon;
using vampierkiller.logia;
using vampirekiller.eevee.util.json;
using vampirekiller.logia.net;
using vampirekiller.logia.stub;
using Json = vampirekiller.eevee.util.json.Json;

namespace vampirekiller.espeon;

public partial class EspeonNet : Node //, Net
{
    public int port = 7000;
    public readonly ENetMultiplayerPeer peer = new();

    [Inject]
    private readonly ICommandManager _commandManager;

    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _Ready()
    {
        this.OnReady();
        this.Inject();
        this.Name = this.GetType().Name;
        Espeon.net = this;

        var error = peer.CreateServer(port);
        if (error != Error.Ok)
        {
            GD.Print("Espeon error cannot host server:" + error.ToString());
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
        GD.Print("EspeonNet ready 3");
    }

    /// <summary>
    /// Receive command packet and pass it to EspeonCommandManager
    /// </summary>
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void onPacketCommand(byte[] bytes)
    {
        GD.Print("Espeon: onPacketCommand: " + bytes);
        string json = Encoding.UTF8.GetString(bytes);
        var command = Json.deserialize<ICommand>(json);

        //ICommand command = null; // deserialize bytes
        _commandManager.handle(command);
    }


    /// <summary>
    /// Runs when a player connects and runs on all peers
    /// </summary>
    private void PeerConnected(long id)
    {
        var player = StubFight.spawnStubPlayer();
        player.playerId = id;
        Universe.fight.creatures.add(player);
        GD.Print("Espeon Peer connected " + id);
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void PeerDisconnected(long id)
    {
        var crea = Universe.fight.creatures.get(c => c.playerId == id);
        if(crea != null)
        {
            Universe.fight.creatures.remove(crea);
            Universe.fight.entities.remove(crea);
            crea.Dispose();
        }
        GD.Print("Espeon Peer disconnected " + id);
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectedToServer()
    {
        GD.Print("Espeon Connected to server");
    }

    /// <summary>
    /// Runs only on client
    /// </summary>
    private void ConnectionFailed()
    {
        GD.Print("Espeon Connection failed");
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    private void ServerDisconnected()
    {
        GD.Print("Espeon Server disconnected");
    }
}
