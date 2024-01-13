using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;
using vampirekiller.eevee.creature;

namespace vampirekiller.eevee.spells;

/// <summary>
/// 
/// </summary>
public class SkillSkin : Identifiable
{
    public ID entityUid { get; set; }
    //public string iconPath { get; set; }
    /// <summary>
    /// Play this scene as an FxNode on the caster
    /// </summary>
    public string sourceFxPath { get; set; }
    /// <summary>
    /// Play this scene as an FxNode on the target if there is one
    /// </summary>
    public string targetFxPath { get; set; }
    /// <summary>
    /// Play this animation on the caster. 
    /// If no animation is set, the creature will use the default CreatureSkin's cast animation.
    /// </summary>
    public string sourceAnimation { get; set; }
    /// <summary>
    /// Play this animation on the target if there is one.
    /// </summary>
    public string targetAnimation { get; set; }
    /// <summary>
    /// Required animation libraries to play the source & target animations. <br></br>
    /// Add these animation libraries to the caster/target if they dont have them. <br></br>
    /// May be better to always load every library available on every creature from the start? -> May have a negative impact, who knows.
    /// </summary>
    public List<string> animationLibraries { get; set; } = new();

    public void Dispose()
    {
    }
}

