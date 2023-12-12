namespace VampireKiller;



public class Config
{
    public VideoSettings videoSettings { get; set; }
    public AudioSettings audioSettings { get; set; }
    public ShortcutSettings shortcutSettings { get; set; }
}

public class VideoSettings
{

}

public class AudioSettings
{
    public int musicVolume { get; set; }
    public int effectVolume { get; set; }
    public int ambianceVolume { get; set; }
    public int interfaceVolume { get; set; }
}

public class ShortcutSettings
{

}

public class GameParameters
{
    public int maxActiveSlots { get; set; } = 5;
    public int maxPassiveSlots { get; set; } = 20;
}
