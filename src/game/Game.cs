using System.Collections.Generic;
using VampireKiller;

namespace Namespace;

public class Game
{

    public GameParameters gameParameters { get; set; }
    
    public List<Player> players { get; set; } = new();
    public List<Enemy> enemies { get; set; } = new();

}
