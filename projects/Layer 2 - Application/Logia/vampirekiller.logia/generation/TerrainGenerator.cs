using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vampirekiller.eevee.campaign.map;

namespace vampirekiller.logia.generation;

public class TerrainGenerator
{
    /*
     * Biomes:
     * - Dungeon
     * - Cavern
     * - Spaceship
     * - Viking village in the snow
     * - 
     */ 


    public Node3D Generate(Room room)
    {
        Node3D root = new Node3D();

        // 1. delimit the area
        //      1. set portals points
        //      2. ~~set spawn points (?)~~ -> nvm it's just anywhere outisde of player's view
        // 2. set walls / barriers around the area
        //      - ocean, rivers
        //      - trees, rocks, mountains, cliffs, big plants
        //      - fences, walls, other buildings
        //      - dark fog + invisible wall xdd (or it tp's you to the otherside?)
        // 3. add doors / portals / roads to the next floor (maybe start with this)
        // 4. set floors
        //      - a layer of terrain like sand, dirt
        //      - a layer of tiles
        // 5. add floor collision props: boxes, pillars, furniture, statues...
        // 6. add no-collision props
        //      - on the walls: banners, paintings, weapons, library, shelves, windows, drapes...
        //          - horror mansion has decals
        //      - on the floors and corners: carpets, crystals, plants, corpses, blood, vases, weapons...
        //      - from the ceiling: chains, noose, chandeliers
        // 7. we also need light sources
        //      - from the ceiling: chandeliers, sun, moon
        //      - from the walls: chandeliers
        //      - from the floors: chandeliers, crystals, lava, campfire, fire, fireplace
        //      - candles placed anywhere on top of props
        //      - modern lights
        // 8. add fog
        //      - dark fog around the player, in its node, to delimit his light radius ? nah just the light from the player himself i think. also it shouldn't affect the player model (different layer)
        // 

        return root;
    }




}
