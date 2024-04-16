using Godot;
using Godot.Sharp.Extras;
using System;
using System.Collections.Generic;
using Util.entity;
using vampierkiller.logia;
using vampirekiller.eevee;
using vampirekiller.eevee.campaign;
using vampirekiller.eevee.campaign.map;
using vampirekiller.glaceon.sapphire.ui.esc_menu.map;
using vampirekiller.glaceon.util;
using VampireKiller.eevee.vampirekiller.eevee;

/// <summary>
/// Sachant que:
/// - le background est 512px wide
/// - Les room icons sont 50px sq
/// </summary>
public partial class UiMap : PanelContainer
{
    private Campaign campaign = new();

    [NodePath]
    public Panel MapCanvas { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        this.Inject();

        var biome = Register.Create<Biome>();
        Diamonds.biomes[biome.entityUid] = biome;
        campaign.settings.MapGenerationSettings.BiomeWeights.Add(biome.entityUid, 1);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void generate()
    {
        campaign.settings.MapGenerationSettings.MaxFloorWidth = 5;
        campaign.settings.MapGenerationSettings.AutoDistributeBranchCountWeights();

        int xInit = 50;
        int xGap = 100;
        int yInit = 50;
        int yGap = 100;

        //Boss,
        //Fight,
        //Merchant, // Can have different merchants based on biome type
        //Market, // all the merchants together
        //Campfire, // small rest
        //Nurse, // giga heal
        //Inn, // giga rest
        //Treasure, // just rewards, no fight
        var icons = new List<String>() { "👾", "👻", "👳‍", "🧙‍", "👨‍🍳", "🏫", "🔥", "🧚‍", "💎", "❓" };

        campaign.Map = new MapGenerator().GenerateByPaths(campaign.settings.MapGenerationSettings, 10);
        //new MapGenerator().AddFloors(campaign.settings.MapGenerationSettings, campaign.Map, 10);

        MapCanvas.RemoveAndQueueFreeChildren();

        for (int i = 0; i < campaign.Map.Floors.Count; i++)
        {
            var floor = campaign.Map.Floors[i];
            for (int j = 0; j < floor.Rooms.Count; j++)
            {
                var room = floor.Rooms[j];
                //float xPos = (room.Index) * xGap + xInit;
                //float yPos = (i + room.VisualOffset.Y) * yGap + yInit;
                float xPos1 = (room.Index) * xGap + xInit;
                float yPos1 = (i) * yGap + yInit;
                float dx1 = room.VisualOffset.X * xGap;
                float dy1 = room.VisualOffset.Y * yGap;

                foreach (var connection in room.Connections)
                {
                    var nextRoom = campaign.Map.Floors[i + 1].GetRoomAt(connection);
                    var nextX = xGap * (connection - room.Index + nextRoom.VisualOffset.X);
                    var nextY = yGap * (1 + nextRoom.VisualOffset.Y);

                    var line = new Line2D();
                    line.AddPoint(new Vector2(dx1 + 25, dy1 + 25));
                    line.AddPoint(new Vector2(nextX + 25, nextY + 25));
                    line.DefaultColor = new Color("1b1b1baa");
                    line.Width = 5;
                    line.JointMode = Line2D.LineJointMode.Round;
                    MapCanvas.AddChild(line);

                    line.Position = new Vector2(xPos1, yPos1);
                }

                var packedScene = AssetCache.Load<PackedScene>("res://vampirekiller.glaceon/sapphire/ui/esc_menu/map/ui_map_node.tscn");
                var roomNode = packedScene.Instantiate<UiMapNode>();
                //var roomNode = new UiMapNode();
                this.MapCanvas.AddChild(roomNode);
                roomNode.Label.Text = icons[(int) room.RoomType];
                roomNode.Position = new Vector2(xPos1 + dx1, yPos1 + dy1);
            }
        }
    }

}
