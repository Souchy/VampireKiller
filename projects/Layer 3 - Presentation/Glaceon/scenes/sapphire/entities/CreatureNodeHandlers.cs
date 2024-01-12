using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.communication.events;
using Util.structures;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.spells;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.stats.schemas.resources;
using vampirekiller.glaceon.util;
using vampirekiller.logia.extensions;
using vampirekiller.umbreon.commands;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.stats;

//namespace scenes.sapphire.entities;

public abstract partial class CreatureNode : CharacterBody3D
{

    #region Event Handlers

    [Subscribe("events.character.size")]
    public void onSetScale(float characterSize)
    {
        this.Scale = new Vector3(characterSize, characterSize, characterSize);
    }

    [Subscribe(HandlerOnCast.EventAnimationCast)]
    public void onAnimationCast(ActionCastActive action, double castTime)
    {
        var skill = action.getActive();
        var anim = skill.skin.sourceAnimation;
        if (anim == null)
        {
            anim = action.getSourceCreature().currentSkin.animations.cast;
        }
        this.CreatureNodeAnimationPlayer.playAnimationOneShot(AnimationState.casting, anim, action.applyActionCast, (float) castTime);
    }

    [Subscribe(DomainEvents.EventDeath)]
    public void onDeath(CreatureInstance crea)
    {
        this.CreatureNodeAnimationPlayer.playAnimationOneShot(AnimationState.death, creatureInstance.currentSkin.animations.death); //.playAnimation(SupportedAnimation.Death);
    }

    [Subscribe(CreatureInstance.EventUpdateStats)]
    public void onStatChanged(CreatureInstance crea, IStat stat)
    {
        // GD.Print("CreatureNode: onStatChanged: " + stat.GetType().Name + " = " + stat.genericValue);
        // todo regrouper les life stats en une liste<type> automatique genre / avoir une annotation [Life] p.ex, etc
        if (stat is CreatureAddedLife || stat is CreatureAddedLifeMax || stat is CreatureBaseLife || stat is CreatureBaseLifeMax || stat is CreatureIncreasedLife || stat is CreatureIncreasedLifeMax)
        {
            updateHPBar();
        }
    }

    [Subscribe]
    public void onActiveSkillSetAt(SmartList<SpellInstance> list, int index, SpellInstance skill)
    {
        foreach (var lib in skill.skin.animationLibraries)
        {
            this.CreatureNodeAnimationPlayer.loadLibrary(lib);
        }
    }
    [Subscribe]
    public void onActiveSkillAdd(SmartList<SpellInstance> list, SpellInstance skill)
    {
        foreach (var lib in skill.skin.animationLibraries)
        {
            this.CreatureNodeAnimationPlayer.loadLibrary(lib);
        }
    }

    [Subscribe]
    public void onItemListAdd(object list, object item)
    {
        // check all statements 
        //      modify mesh / material / etc si nécessaire
        recalculateCache();
    }
    [Subscribe]
    public void onItemListRemove(object list, object item)
    {
        recalculateCache();
    }
    [Subscribe]
    public void onStatusListAdd(object list, object item)
    {
        recalculateCache();
    }

    [Subscribe(nameof(SmartList<Status>.remove))]
    public void onStatusListRemove(SmartList<Status> list, Status item)
    {
        //  TODO recalculate cache on status change/item change
        recalculateCache();
    }
    [Subscribe(DomainEvents.EventDamage)]
    public void onDamage(int value)
    {
        var popup = AssetCache.Load<PackedScene>("res://scenes/sapphire/ui/components/UiResourcePopup.tscn").Instantiate<UiResourcePopup>();
        popup.value = value;
        this.AddChild(popup, true);
        this.CreatureNodeAnimationPlayer.playAnimationOneShot(AnimationState.receiveHit, creatureInstance.currentSkin.animations.receiveHit);
    }
    #endregion

}
