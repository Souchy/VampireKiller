using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using Util.communication.events;
using vampirekiller.glaceon.util;
using vampirekiller.logia;

namespace scenes.sapphire.entities.component;

/// <summary>
/// A Character model corresponds to a character scene. 
/// Examples: 
/// - Character_ForestGuardian_01.glb (glb = default import), 
/// - character_mystic_01.tscn (added arms), 
/// - character_spirit_demon.tscn (multiple mesh),
/// - character_ghost_01.tscn (added the right material)
/// </summary>
public partial class CharacterModelNode : Node3D
{
    [NodePath] public Skeleton3D GeneralSkeleton { get; set; }
    [NodePath] public AnimationPlayer AnimationPlayer { get; set; }

    public IEnumerable<MeshInstance3D> MeshInstances { get; set; }
    public IEnumerable<BoneAttachment3D> BoneAttachments { get; set; }

    public override void _EnterTree()
    {
        // Animation script
        if(!this.HasNode(nameof(AnimationPlayer))) 
            return;
        this.AnimationPlayer = this.GetNode(nameof(AnimationPlayer)).SafelySetScript<CreatureNodeAnimationPlayer>(Paths.entities + nameof(CreatureNodeAnimationPlayer) + ".cs");
    }

    public override void _Ready()
    {
        if (!this.HasNode(nameof(AnimationPlayer)))
            return;
        this.OnReady();
        // Meshs & BoneAttachments
        MeshInstances = GeneralSkeleton.GetChildren<MeshInstance3D>().Where(n => n != null);
        BoneAttachments = GeneralSkeleton.GetChildren<BoneAttachment3D>().Where(n => n != null);

        if (Universe.isOnline && !this.Multiplayer.IsServer())
            return;

        // Setup synchronizer
        var sync = new MultiplayerSynchronizer();
        var prop = nameof(AnimationPlayer) + ":" + AnimationPlayer.PropertyName.CurrentAnimation;
        sync.ReplicationConfig = new SceneReplicationConfig();
        sync.ReplicationConfig.AddProperty(prop);
        this.AddChild(sync, true);

        // Setup bone attachments spawners
        foreach (var bone in BoneAttachments)
        {
            var spawner = new MultiplayerSpawner();
            spawner.AddToGroup("CharacterAttachmentSpawners");
            spawner.Name = bone.Name + "Spawner";
            spawner.SpawnPath = this.GetPathTo(bone);
            spawner._SpawnableScenes = AssetCache.skills.ToArray();
            this.AddChild(spawner, true);
        }

    }

    /// <summary>
    /// Maybe TODO ?
    /// </summary>
    [Subscribe("events.character.attach")]
    public void onAttach(string boneName, string scenePathToAttach)
    {
        Node scene = AssetCache.Load<PackedScene>(scenePathToAttach).Instantiate<Node>();
        this.BoneAttachments.Single(b => b.Name == boneName).AddChild(scene, true);
    }

    [Subscribe("events.character.material")]
    public void onSetMaterial(string meshName, string materialPath)
    {
        Material mat = AssetCache.Load<Material>(materialPath);
        this.MeshInstances.Single(m => m.Name == meshName).MaterialOverride = mat;
    }

}
