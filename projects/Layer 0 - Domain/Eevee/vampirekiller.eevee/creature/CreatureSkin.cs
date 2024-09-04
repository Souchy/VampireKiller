using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.entity;

namespace vampirekiller.eevee.creature;

/// <summary>
/// 
/// </summary>
public class CreatureSkin : Identifiable
{
    public ID entityUid { get; set; }
    public ID nameId { get; set; }
    public ID descriptionId { get; set; }
    /// <summary>
    /// Skin icon
    /// </summary>
    public string iconPath { get; set; }
    /// <summary>
    /// Skin/Model scene containing the mesh + animation player + bone attachments
    /// Or just Mesh resource for crowd nodes
    /// </summary>
    public string scenePath { get; set; }
    /// <summary>
    /// Animations to play on certain states/actions
    /// </summary>
    public CreatureAnimationData animations { get; set; } = new();
    /// <summary>
    /// Animation libraries to load into the creature
    /// </summary>
    public List<string> animationLibraries { get; set; } = new();

    public void Dispose()
    {
    }
}

public class CreatureAnimationData
{
    public string idle;
    /// <summary>
    /// ex: some gestures like looking around
    /// </summary>
    public string[] idleOneShots = new string[0];
    public string run;
    public string walk;
    public string cast;
    public string receiveHit;
    public string death;
    public string victory;
    public string defeat;
}