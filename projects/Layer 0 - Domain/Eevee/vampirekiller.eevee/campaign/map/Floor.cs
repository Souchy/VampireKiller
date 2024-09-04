using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vampirekiller.eevee.campaign.map;

public class Floor
{
    public int Index { get; set; } = 0;
    public FloorType FloorType { get; set; }
    public List<Room> Rooms {  get; set; } = new();
    public bool HasRoomClose(int roomIndex)
    {
        return Rooms.Any(r => Math.Abs(r.Index - roomIndex) <= 1);
    }
    public IEnumerable<Room> GetRoomClose(int roomIndex)
    {
        return Rooms.Where(r => Math.Abs(r.Index - roomIndex) <= 1);
    }
    public Room? GetRoomAt(int index)
    {
        return Rooms.FirstOrDefault(r => r.Index == index);
    }
}

public enum FloorType
{
    /// <summary>
    /// Each room is random
    /// </summary>
    Normal,
    /// <summary>
    /// Only one room, which is a boss
    /// </summary>
    Boss, 
    /// <summary>
    /// Proposes a choice of x different treasure. Pokemon moment, ex choose str vs dex vs int
    /// </summary>
    Treasure,
}