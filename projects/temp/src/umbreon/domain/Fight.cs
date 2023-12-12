
using System.Collections.Generic;
using aaaaaaaa;
using VampireKiller;

public class Fight
{
    public Dictionary<ObjectId, Player> players {get; set; } = new();
    public Dictionary<ObjectId, Enemy> enemies { get; set; } = new();
}